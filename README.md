# Brahma.FSharp.MatrixMultiply

This tool multiplies dense matrices using [Brahma.FSharp](https://github.com/YaccConstructor/Brahma.FSharp)

---

## Builds


GitHub Actions |
:---: |
[![GitHub Actions](https://github.com/kirillgarbar/Brahma.FSharp.MatrixMultiply/workflows/Build%20master/badge.svg)](https://github.com/kirillgarbar/Brahma.FSharp.MatrixMultiply/actions?query=branch%3Amaster) |
[![Build History](https://buildstats.info/github/chart/kirillgarbar/Brahma.FSharp.MatrixMultiply)](https://github.com/kirillgarbar/Brahma.FSharp.MatrixMultiply/actions?query=branch%3Amaster) |

---

## How to use it

This project implements a custom console interface via [Argu](https://github.com/fsprojects/Argu)
The available console commands are listed below:
#
    --firstmatrix <file>  File that contains first matrix
    --secondmatrix <file> File that contains second matrixe
    --outputfile <file>   File that will contain the result of multiplication
    --platform <nvidia|amd|any>
                          Platform for openCL. Make sure openCL is installed. Run "clinfo" to check your platforms.
                          Options:"NVIDIA", "AMD", "Any"
    --help                display this list of options.

### Matrix format

Matrices are stored in text files (.txt)

#### Correct format
#
    1.0 27.543 12.5
    78.4 14.15 88.2
    100.0 15.7 20.4
Using integers is also allowed
#
    1 27 12.5
    78.4 14 88.2
    100.0 15 20.4
#### Incorrect format
Non-rectangular matrix
# 
    1 2 3
    1 2 3
    1 2  
    4
Not numeric matrix
#
    1 2 3
    a b c
    1 2 3

## Requirments

  * .NET 5.0 or greater
  * OpenCL

## Directory structure

```
Brahma.FSharp.MatrixMultiply
├── .config - Dotnet tools
├── .github - GitHub Actions CI setup 
├── src
│   └── Brahma.FSharp.MatrixMultiply - CLI, MatrixMultiply, MatrixIO modules
├── tests
│   └── Brahma.FSharp.MatrixMultiply.Tests - Tests for all modules
└── Brahma.FSharp.MatrixMultiply.sln - Main solution file
```