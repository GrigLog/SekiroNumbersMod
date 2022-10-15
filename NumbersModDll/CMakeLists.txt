cmake_minimum_required(VERSION 3.22)
project(NumbersMod)
set(CMAKE_CXX_STANDARD 23)

include (GenerateExportHeader)

add_subdirectory(imgui)
add_subdirectory(kiero-cmake)
add_library(${PROJECT_NAME} MODULE main.cpp NumbersMod.cpp V3.hpp data.h data_reader.hpp drawer.cpp config.h drawer.h config.cpp)
target_link_libraries(${PROJECT_NAME} sub::imgui sub::kiero d3d11.lib)
#MAKE SURE THAT YOUR COMPILER IS TARGETING x86-64 AND NOTHING ELSE
