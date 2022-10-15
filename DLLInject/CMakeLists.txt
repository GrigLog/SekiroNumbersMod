cmake_minimum_required(VERSION 3.22)
project(DllInject)
set(CMAKE_CXX_STANDARD 23)

add_executable(${PROJECT_NAME} DLLInjector.cpp)