# EasyUDE Project Introduction

<p align="right">
  <a >English</a> |
  <a href="./README.zh.md">中文</a> 
</p>

[![](https://img.shields.io/nuget/dt/EasyUDE?color=004880&label=downloads&logo=NuGet)](https://www.nuget.org/packages/EasyUDE/)
[![](https://img.shields.io/nuget/vpre/EasyUDE?color=%23004880&label=NuGet&logo=NuGet)](https://www.nuget.org/packages/EasyUDE/)
[![GitHub](https://img.shields.io/github/license/ArchieWoo/EasyUDE?color=%231281c0)](LICENSE)

## Overview

**EasyUDE** is a lightweight .NET utility library focused on character encoding detection and conversion. It helps developers quickly identify the original encoding format of text data and efficiently convert it to target encoding formats. The library features built-in confidence detection logic based on statistical analysis and character distribution patterns, significantly improving encoding detection accuracy.

EasyUDE can process data from various sources including file streams, individual strings, string arrays, and 2D string arrays, delivering results in the desired format. Its core strength lies in analyzing character weights (such as language-specific character frequency and double-byte sequence distribution) combined with advanced state machine models and contextual analysis algorithms, effectively preventing garbled text caused by incorrect encoding detection.

### Key Features

1. **High-Precision Encoding Detection**  
   EasyUDE employs statistical models based on character distribution, matching against weighted character patterns for different languages and encodings. This mechanism effectively distinguishes between similar encodings (like UTF-8 and ISO-8859 series), significantly reducing false positives.

2. **Confidence Scoring**  
   During detection, EasyUDE calculates a confidence score for each potential encoding format. Results are only returned when scores exceed a certain threshold, ensuring reliable detection and minimizing garbled text risks.

3. **Multilingual Support**  
   Optimized models for multiple languages (Chinese, Japanese, Korean, Greek, Hebrew, etc.) enable accurate encoding detection for mixed-language content.

4. **Flexible Input Types**  
   Supports various input formats:
   - File streams (Stream)
   - Individual strings (string)
   - 1D string arrays (string[])
   - 2D string arrays (string[,])
   - Direct file path reading/writing

5. **Efficient Encoding Conversion**  
   After detecting the original encoding, EasyUDE provides seamless conversion to target encodings (UTF-8, GB18030, Big5, etc.), automatically handling unmappable characters to ensure clean output.

### Confidence Detection Advantages

Traditional encoding detection methods relying on simple rules or fixed patterns often fail with multilingual content or special characters. EasyUDE's confidence detection solves these issues through:

1. **Character Frequency Analysis**  
   Using predefined frequency tables (e.g., GB18030 for Chinese, Shift_JIS for Japanese) to match input data against target encodings.

2. **Double-Byte Sequence Analysis**  
   For multi-byte encodings (GB18030, EUC-KR, etc.), analyzes byte sequence patterns for higher accuracy.

3. **Context Awareness**  
   Leverages contextual clues (punctuation, spaces, line breaks) for smarter detection, especially effective with short texts or mixed-language content.

4. **Dynamic Threshold Adjustment**  
   Confidence thresholds adapt based on input data length and complexity, maintaining high precision even with small datasets.

---

## Core Functionality

### 1. **Supported Encodings**

#### (1) Detection Support
EasyUDE automatically detects:
- **Unicode Encodings**:
  - UTF-8 (`UTF8`)
  - UTF-16LE (`UTF16_LE`) and UTF-16BE (`UTF16_BE`)
  - UTF-32BE (`UTF32_BE`) and UTF-32LE (`UTF32_LE`)
  - UCS-4 (non-standard BOM: `UCS4_3412`, `UCS4_2413`)
- **Regional Encodings**:
  - Cyrillic: `WIN1251`, `ISO8859_5`, `KOI8R`, `IBM855`, `IBM866`, `MAC_CYRILLIC`
  - East-European: `ISO8859_2`
  - Greek: `WIN1253`, `ISO_8859_7`
  - Hebrew: `WIN1255`, `ISO8859_8`
  - Thai: `TIS620` (currently disabled)
- **Chinese Encodings**:
  - GB series: `GB18030` (compatible with `GB2312`, `GBK`)
  - Big5 (Traditional Chinese)
  - HZ-GB-2312
- **Japanese/Korean Encodings**:
  - Japanese: `Shift_JIS`, `EUC-JP`, `ISO-2022-JP`
  - Korean: `EUC-KR`, `ISO-2022-KR`

#### (2) Conversion Support
EasyUDE converts between all detected encodings and:
- ASCII
- UTF-8/16/32
- GB18030/GBK/GB2312
- Big5
- Shift_JIS
- EUC-KR/EUC-JP
- ISO-8859 series

---

### 2. **Multi-Format Input Support**
Flexible input handling for different scenarios:
- **Stream**: Directly read from files/network streams
- **string**: Process individual strings
- **string[]**: Process 1D string arrays
- **string[,]**: Process 2D string arrays
- **File Paths**: Direct file read/write operations

---

### 3. **File-Level Encoding Conversion**
Provides direct file-to-file encoding conversion capabilities.

---

## Key Applications

1. **Multilingual Text Processing**
   Accurately identifies and converts mixed-language text encodings to prevent garbled text.

2. **Batch File Conversion**
   Efficiently handles bulk encoding conversion tasks (e.g., GB18030 → UTF-8).

3. **Data Cleaning & Preprocessing**
   Standardizes text encoding for data science/text analysis workflows.

4. **Cross-Platform Development**
   Bridges encoding differences between systems (e.g., Windows GB18030 vs Linux UTF-8).

---

## Technical Highlights

1. **High Performance**
   Optimized state machine models and character distribution algorithms ensure efficiency at scale.

2. **Flexibility**
   Supports diverse input types and target encodings.

3. **Robustness**
   Automatically replaces unmappable characters (e.g., `�`) to preserve data integrity.

4. **Ease of Use**
   Simple API requires minimal encoding expertise.

---

## Roadmap

1. **More Encodings**
   Add support for niche encodings like TIS-620 (Thai).

2. **Enhanced Error Handling**
   Improve error logging and exception handling.

3. **GUI Tool**
   Develop user-friendly graphical interface for non-technical users.

---

With **EasyUDE**, tackle encoding challenges effortlessly - whether in development, data analysis, or daily file processing!

## Project Structure  
```plaintext
Project Root/
├── EasyUDE/                # Source Code
│   ├── Assets/             # Resources
│   ├── Contracts/          # Interfaces
│   ├── Enums/              # Enumerations
│   ├── Helpers/            # Extensions
│   ├── Models/             # Data Models
│   ├── README.zh_CN.md     # Chinese Documentation
│   ├── CharserHandler      # Core Functionality
│   ├── ICharserHandler     # Core Interface
│   ├── TargetEncoding      # Encoding Enum
│   └── EasyDbc.csproj      # Project File
├── EasyDbc.Test/           # Unit Tests
│   ├── Data/               # Test Data
│   └── EasyDbc.Test.csproj # Test Project
├── README.md               # Main Documentation
└── LICENSE                 # License
```

# EasyUDE Quick Start Guide
## Overview
**EasyUDE** simplifies encoding detection and conversion in .NET. This guide demonstrates basic usage for handling various data types.

---

## Installation
### 1. Install Package
## Via NuGet:
```bash
dotnet add package EasyUDE
```
## Namespace
```csharp
using EasyUDE;
```
# Encoding Detection
## Supported Input Types
### 1. **Stream**
```csharp
ICharsetHandler handler = new CharsetHandler();
using (FileStream inputStream = new FileStream("input.txt", FileMode.Open, FileAccess.Read))
{
    string detectedEncoding = handler.GetCharset(inputStream);
    Console.WriteLine($"Detected Encoding: {detectedEncoding}");
}
```
### 2. **string[,] (2D Array)**
```csharp
string[,] strings = 
{
    { "Sample Text 1", "Test Content 1" },
    { "こんにちは", "Hello, World!" }
};

ICharsetHandler handler = new CharsetHandler();
string detectedEncoding = handler.GetCharset(strings);
Console.WriteLine($"Detected Encoding: {detectedEncoding}");
```
### 3. **string[] (1D Array)**
```csharp
string[] strings = { "Sample Text 1", "Test Content 1", "こんにちは" };
string detectedEncoding = handler.GetCharset(strings);
Console.WriteLine($"Detected Encoding: {detectedEncoding}");
```
### 4. **string**
```csharp
string inputString = "Sample Text";
string detectedEncoding = handler.GetCharset(inputString);
Console.WriteLine($"Detected Encoding: {detectedEncoding}");
```
# Encoding Conversion
## Supported Input Types
### 1. **Stream Conversion**
```csharp
ICharsetHandler handler = new CharsetHandler();
using (FileStream inputStream = new FileStream("input.txt", FileMode.Open, FileAccess.Read))
{
    using (Stream convertedStream = handler.DetectAndConvert(inputStream, TargetEncoding.UTF_8))
    {
        using (FileStream outputStream = new FileStream("output.txt", FileMode.Create, FileAccess.Write))
        {
            convertedStream.CopyTo(outputStream);
        }
    }
}
```
### 2. **2D Array Conversion**
```csharp
string[,] strings = 
{
    { "测试内容1", "Test Content 1" },
    { "こんにちは", "Hello, World!" }
};

ICharsetHandler handler = new CharsetHandler();
string[,] convertedStrings = handler.DetectAndConvert(strings, TargetEncoding.UTF_8);

foreach (var item in convertedStrings)
{
    Console.WriteLine(item);
}
```
### 3. **1D Array Conversion**
```csharp
string[] strings = { "测试内容1", "Test Content 1", "こんにちは" };

ICharsetHandler handler = new CharsetHandler();
string[] convertedStrings = handler.DetectAndConvert(strings, TargetEncoding.UTF_8);

foreach (var item in convertedStrings)
{
    Console.WriteLine(item);
}
```
### 4. **String Conversion**
```csharp
string inputString = "测试内容1";

ICharsetHandler handler = new CharsetHandler();
string convertedString = handler.DetectAndConvert(inputString, TargetEncoding.UTF_8);

Console.WriteLine(convertedString);
```