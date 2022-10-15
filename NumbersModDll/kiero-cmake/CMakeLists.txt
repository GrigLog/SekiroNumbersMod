cmake_minimum_required(VERSION 3.5)
project(SekiroNumbersMod LANGUAGES CXX VERSION 1.2.12)

# --------------------------------------------------------------------------------------------------------
# Library options
# --------------------------------------------------------------------------------------------------------

option(kiero_minhook "Whether or not Kiero should use MinHook" ON)

set(kiero_backend_type D3D9 D3D10 D3D11 D3D12 OpenGL Vulkan)
set(kiero_backend "D3D11")
set(kiero_backend None CACHE STRING "The backend kiero should use.")
set_property(CACHE kiero_backend PROPERTY STRINGS ${kiero_backend_type})

if (NOT kiero_backend IN_LIST kiero_backend_type)
    message(FATAL_ERROR "Kiero backend must be one of ${kiero_backend_type}")
endif()

# --------------------------------------------------------------------------------------------------------
# Fix up Kiero
# --------------------------------------------------------------------------------------------------------

file(READ "kiero.h" kiero_data)
string(REGEX REPLACE "(#define (KIERO_INCLUDE|KIERO_USE_MINHOOK)[^\n]+)" "" kiero_data "${kiero_data}")
file(WRITE "../kiero-fixed/include/kiero.h" "${kiero_data}")

file(READ "kiero.cpp" kiero_src)
string(REGEX REPLACE "(#include \"kiero.h\")\n" "#include \"../include/kiero.h\"\n" kiero_src "${kiero_src}")
string(REGEX REPLACE "(# include \"minhook/include/MinHook\.h\"\n)" "#include <MinHook.h>\n" kiero_src "${kiero_src}")
file(WRITE "../kiero-fixed/src/kiero.cpp" "${kiero_src}")

# --------------------------------------------------------------------------------------------------------
# Create library
# --------------------------------------------------------------------------------------------------------

add_library(${PROJECT_NAME} STATIC "../kiero-fixed/src/kiero.cpp")
add_library(sub::kiero ALIAS ${PROJECT_NAME})
# target_link_libraries(${PROJECT_NAME} imgui d3d11.lib)
target_include_directories(${PROJECT_NAME} PUBLIC "../kiero-fixed/include/")

if (CMAKE_CXX_COMPILER_ID MATCHES "Clang")
    target_compile_options(${PROJECT_NAME} PRIVATE "-Wno-microsoft-cast")
endif()

# --------------------------------------------------------------------------------------------------------
# Add defines depending on options
# --------------------------------------------------------------------------------------------------------

if (${kiero_backend} STREQUAL "D3D9")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_D3D9)
elseif(${kiero_backend} STREQUAL "D3D10")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_D3D10)
elseif(${kiero_backend} STREQUAL "D3D11")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_D3D11)
elseif(${kiero_backend} STREQUAL "D3D12")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_D3D12)
elseif(${kiero_backend} STREQUAL "OpenGL")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_OPENGL)
elseif(${kiero_backend} STREQUAL "Vulkan")
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_INCLUDE_VULKAN)
endif()

# --------------------------------------------------------------------------------------------------------
# Link minhook if required
# --------------------------------------------------------------------------------------------------------

if (kiero_minhook)
    find_package(minhook QUIET)
    
    if (NOT TARGET minhook::minhook)
        message(STATUS "Making MinHook available")
        
        include(FetchContent)
        FetchContent_Declare(minhook GIT_REPOSITORY "https://github.com/TsudaKageyu/minhook")

        FetchContent_MakeAvailable(minhook)
        add_library(minhook::minhook ALIAS minhook) # minhook::minhook is the package name we get with vpckg.
    endif()

    message(STATUS "Linking MinHook")
    target_link_libraries(${PROJECT_NAME} PRIVATE minhook::minhook)
    
    target_compile_definitions(${PROJECT_NAME} PUBLIC KIERO_USE_MINHOOK=1)
endif()
