# EasyUDE 项目简介

<p align="right">
  <a href="./README.en.md">English</a> |
  <a >中文</a> 
</p>

[![](https://img.shields.io/nuget/dt/EasyUDE?color=004880&label=downloads&logo=NuGet)](https://www.nuget.org/packages/EasyUDE/)
[![](https://img.shields.io/nuget/vpre/EasyUDE?color=%23004880&label=NuGet&logo=NuGet)](https://www.nuget.org/packages/EasyUDE/)
[![GitHub](https://img.shields.io/github/license/ArchieWoo/EasyUDE?color=%231281c0)](LICENSE)


## 概述

**EasyUDE** 是一个轻量级的 .NET 工具库，专注于字符串编码格式的检测与转换。它不仅能够帮助开发者快速识别文本数据的原始编码格式，并将其高效转换为目标编码格式，还内置了一套基于统计学和字符分布分析的可信度检测逻辑，从而显著提升编码检测的准确性。

无论是从文件流、单个字符串、字符串数组还是二维字符串数组中读取数据，EasyUDE 都能高效处理并输出符合需求的结果。其核心优势在于通过分析常用字符的权重（如语言特定的字符频率、双字节序列分布等），结合先进的状态机模型和上下文分析算法，有效避免因编码格式检测不正确而导致的文字乱码问题。

### 核心特点

1. **高精度编码检测**  
   EasyUDE 使用基于字符分布的统计模型，按照不同语言和编码格式中常用字符的权重进行匹配。这种机制可以有效区分相似编码（如 UTF-8 和 ISO-8859 系列），从而大幅降低误判率。

2. **可信度评估**  
   在检测过程中，EasyUDE 会为每种可能的编码格式计算一个可信度分数（Confidence Score）。只有当可信度达到一定阈值时，才会认定为最终的检测结果。这种设计确保了检测结果的可靠性，减少了因错误检测导致的乱码风险。

3. **多语言支持**  
   EasyUDE 内置了针对多种语言的优化模型（如中文、日文、韩文、希腊语、希伯来语等），能够准确识别多语言混合内容的编码格式。

4. **灵活的输入类型**  
   支持多种输入类型，包括：
   - 文件流（Stream）
   - 单个字符串（string）
   - 一维字符串数组（string[]）
   - 二维字符串数组（string[,]）
   - 直接读取和写入文件路径

5. **高效的编码转换**  
   在检测到原始编码后，EasyUDE 提供了无缝的编码转换功能，支持将数据转换为目标编码格式（如 UTF-8、GB18030、Big5 等）。转换过程会自动处理不可映射字符，确保输出内容完整且无乱码。

### 可信度检测逻辑的优势

传统的编码检测方法通常依赖于简单的规则或固定模式匹配，容易在处理多语言混合内容或特殊字符时出现误判。而 EasyUDE 的可信度检测逻辑通过以下方式解决了这些问题：

1. **字符频率分析**  
   针对不同语言的文本，EasyUDE 使用预定义的字符频率表（如中文常用的 GB18030 字符分布、日文的 Shift_JIS 分布等），计算输入数据与目标编码的匹配程度。

2. **双字节序列分析**  
   对于多字节编码（如 GB18030、EUC-KR 等），EasyUDE 会分析双字节序列的分布特征，进一步提高检测的准确性。

3. **上下文感知**  
   结合上下文信息（如标点符号、空格、换行符等），EasyUDE 能够更智能地判断编码格式，尤其是在处理短文本或混合语言内容时表现尤为出色。

4. **动态调整阈值**  
   EasyUDE 的检测逻辑会根据输入数据的长度和复杂度动态调整可信度阈值，确保在处理小规模数据时也能保持高精度。

---

## 核心功能

### 1. **支持的编码格式**

#### **(1) 编码检测**
EasyUDE 支持以下编码格式的自动检测：
- **Unicode 编码**：
  - UTF-8 (`UTF8`)
  - UTF-16LE (`UTF16_LE`) 和 UTF-16BE (`UTF16_BE`)
  - UTF-32BE (`UTF32_BE`) 和 UTF-32LE (`UTF32_LE`)
  - UCS-4（非标准 BOM：`UCS4_3412` 和 `UCS4_2413`）
- **区域性编码**：
  - Cyrillic (斯拉夫语系)：`WIN1251`、`ISO8859_5`、`KOI8R`、`IBM855`、`IBM866`、`MAC_CYRILLIC`
  - East-European (东欧语言)：`ISO8859_2`
  - Greek (希腊语)：`WIN1253`、`ISO_8859_7`
  - Hebrew (希伯来语)：`WIN1255`、`ISO8859_8`
  - Thai (泰语)：`TIS620`（目前未启用检测）
- **中文编码**：
  - GB 系列：`GB18030`（兼容 `GB2312` 和 `GBK`）
  - Big5（繁体中文）
  - HZ-GB-2312
- **日文和韩文编码**：
  - 日文：`Shift_JIS`、`EUC-JP`、`ISO-2022-JP`
  - 韩文：`EUC-KR`、`ISO-2022-KR`

#### **(2) 编码转换**
EasyUDE 支持将检测到的原始编码格式转换为任意目标编码格式，包括但不限于：
- ASCII
- UTF-8、UTF-16、UTF-32
- GB18030、GBK、GB2312
- Big5
- Shift_JIS
- EUC-KR、EUC-JP
- ISO-8859 系列（如 ISO-8859-1、ISO-8859-5 等）

---

### 2. **多类型输入支持**
EasyUDE 提供灵活的输入方式，适用于不同的应用场景：
- **Stream**：直接从文件流或网络流中读取数据。
- **string**：处理单个字符串。
- **string[]**：处理一维字符串数组。
- **string[,]**：处理二维字符串数组。
- **文件路径**：支持直接读取和写入文件。

---

### 3. **文件级别的编码转换**
EasyUDE 提供文件级别的编码转换功能，可以直接读取源文件内容，检测其编码格式并将其转换为目标编码格式后保存为新文件。

---

## 主要应用场景

1. **多语言文本处理**
   在处理包含多语言混合内容的文本时，EasyUDE 能够准确识别文本的编码格式，并将其转换为统一的目标编码格式，避免因编码不一致导致的乱码问题。

2. **文件批量转换**
   对于需要批量处理文件编码的任务（如将 GB18030 编码的文件转换为 UTF-8），EasyUDE 提供高效的解决方案。

3. **数据清洗与预处理**
   在数据科学或文本分析任务中，EasyUDE 可以帮助清理和标准化文本数据的编码格式，为后续处理提供干净的数据源。

4. **跨平台开发**
   在跨平台开发中，不同系统可能使用不同的默认编码格式（如 Windows 的 GB18030 和 Linux 的 UTF-8）。EasyUDE 能够在不同平台间无缝转换编码，确保数据一致性。

---

## 技术亮点

1. **高性能**
   EasyUDE 内部采用优化的状态机模型和字符分布分析算法，能够在处理大规模数据时保持高效性能。

2. **灵活性**
   支持多种输入类型和目标编码格式，满足不同场景下的需求。

3. **鲁棒性**
   在处理不可映射字符时，EasyUDE 会自动替换为占位符（如 `�`），确保输出内容不会丢失关键信息。

4. **易用性**
   提供简单直观的 API 接口，开发者无需深入了解编码细节即可轻松完成编码检测和转换任务。

---

## 未来计划

1. **支持更多编码格式**
   增加对更多非主流编码格式的支持，如 TIS-620（泰文）和其他区域性编码。

2. **增强错误处理**
   提供更详细的错误日志和异常处理机制，帮助开发者快速定位和解决问题。

3. **图形化界面工具**
   开发一个基于 GUI 的工具，方便非技术用户进行文件编码检测和转换。

---

通过 **EasyUDE**，您可以轻松应对各种编码相关的挑战，无论是在开发、数据分析还是日常文件处理中，都能显著提升效率和准确性！

## 项目结构  
```plaintext
项目根目录/
├── EasyUDE/                # 源代码文件夹
│   ├── Assets/             # 图片等附加资源
│   ├── Contracts/          # 接口文件
│   ├── Enums/              # 枚举
│   ├── Helpers/            # 扩展
│   ├── Models/             # 数据模型
│   ├── README.zh_CN.md     # 中文项目说明文件
│   ├── CharserHandler      # 主要功能类
│   ├── ICharserHandler     # 主要功能类接口
│   ├── TargetEncoding      # 目标编码格式枚举
│   └── EasyDbc.csproj      # 项目文件
├── EasyDbc.Test/           # 单元测试文件夹
│   ├── Data/               # 编码测试文件夹
│   └── EasyDbc.Test.csproj # 测试项目文件
├── README.md               # 项目说明文件
└── LICENSE                 # 许可证文件
```
---

# EasyUDE 快速入门指南

## 概述
**EasyUDE** 是一个轻量级的 .NET 工具库，专注于字符串编码格式的检测与转换。它支持多种常见的编码格式（如 UTF-8、GB18030、Big5 等），并提供灵活的接口来处理不同类型的输入数据（如文件流、字符串数组、单个字符串等）。通过本指南，您将快速了解如何使用 EasyUDE 进行编码检测和转换。

---

## 安装与配置

### 1. 安装依赖
确保您的项目已引用 `EasyUDE` 库。如果使用 NuGet，请运行以下命令：
```bash
dotnet add package EasyUDE
```

## 引用命名空间

在使用 **EasyUDE** 时，首先需要引用以下命名空间：

```csharp
using EasyUDE;
```

# 编码检测

## 概述
**EasyUDE** 提供了强大的编码检测功能，能够自动识别多种常见的字符编码格式。通过使用 `ICharsetHandler.GetCharset` 方法，开发者可以轻松检测输入数据的编码格式，支持多种输入类型（如文件流、字符串数组、单个字符串等）。

---

## 支持的输入类型

### 1. **Stream**
- 适用于文件流或其他字节流。
- 示例：检测文件流的编码格式。

```csharp
ICharsetHandler handler = new CharsetHandler();
using (FileStream inputStream = new FileStream("input.txt", FileMode.Open, FileAccess.Read))
{
    string detectedEncoding = handler.GetCharset(inputStream);
    Console.WriteLine($"Detected Encoding: {detectedEncoding}");
}
```
## 2. **string[,]**
```csharp
using EasyUDE;

class Program
{
    static void Main()
    {
        // 定义一个二维字符串数组
        string[,] strings = 
        {
            { "测试内容1", "Test Content 1" },
            { "こんにちは", "Hello, World!" }
        };

        // 初始化 CharsetHandler 实例
        ICharsetHandler handler = new CharsetHandler();

        // 检测编码格式
        string detectedEncoding = handler.GetCharset(strings);
        Console.WriteLine($"Detected Encoding: {detectedEncoding}");
    }
}
```
## 3. **string[]**
```csharp
using EasyUDE;

ICharsetHandler handler = new CharsetHandler();
string[] strings = { "测试内容1", "Test Content 1", "こんにちは" };
string detectedEncoding = handler.GetCharset(strings);
Console.WriteLine($"Detected Encoding: {detectedEncoding}");
```

## 4. **string**
```csharp
using EasyUDE;

ICharsetHandler handler = new CharsetHandler();
string inputString = "测试内容1";
string detectedEncoding = handler.GetCharset(inputString);
Console.WriteLine($"Detected Encoding: {detectedEncoding}");
```

---
# 编码转换功能

## 概述
**EasyUDE** 提供了强大的编码转换功能，能够将输入数据从原始编码格式转换为目标编码格式。通过 `ICharsetHandler` 接口，开发者可以灵活地处理多种输入类型（如文件流、字符串数组、单个字符串等），并支持目标编码的自动检测和转换。

---

## 支持的输入类型

### 1. **Stream**
- 适用于文件流或其他字节流。
- 示例：将文件流从原始编码转换为目标编码。

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
### 2. **string[,]（二维字符串数组）**
- 适用于处理一维字符串数组。
- 示例：将一维字符串数组从原始编码转换为目标编码。

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
### 3. **string[]（一维字符串数组）**
- 适用于处理一维字符串数组。
- 示例：将一维字符串数组从原始编码转换为目标编码。

```csharp
string[] strings = { "测试内容1", "Test Content 1", "こんにちは" };

ICharsetHandler handler = new CharsetHandler();
string[] convertedStrings = handler.DetectAndConvert(strings, TargetEncoding.UTF_8);

foreach (var item in convertedStrings)
{
    Console.WriteLine(item);
}
```
### 4. **string（单个字符串**
- 适用于处理单个字符串。
- 示例：将单个字符串从原始编码转换为目标编码。

```csharp
string inputString = "测试内容1";

ICharsetHandler handler = new CharsetHandler();
string convertedString = handler.DetectAndConvert(inputString, TargetEncoding.UTF_8);

Console.WriteLine(convertedString);
```

---

# 贡献

欢迎贡献！请随时提交拉取请求（pull requests）来改进这个库。

---