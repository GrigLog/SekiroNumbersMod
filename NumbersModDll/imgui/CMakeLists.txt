cmake_minimum_required(VERSION 3.22)
project(imgui)
set(CMAKE_CXX_STANDARD 23)

file(GLOB all_sources "*.cpp")
add_library(${PROJECT_NAME} ${all_sources})
add_library(sub::imgui ALIAS ${PROJECT_NAME})