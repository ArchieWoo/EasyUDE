using EasyUDE.Helpers;

namespace EasyUDE
{
    public class CharsetHandler : ICharsetHandler
    {
        private CharsetDetector charsetDetector = new CharsetDetector();
        private CharsetConverter charsetConverter = new CharsetConverter();
        /// <summary>
        /// Detects the encoding format of the input stream and converts it to the target encoding.
        /// </summary>
        /// <param name="inputStream">The input stream</param>
        /// <param name="targetEncodingName">The name of the target encoding</param>
        /// <returns>A Stream containing the converted data (if no encoding format is detected, returns an empty Stream)</returns>
        public Stream DetectAndConvert(Stream inputStream, string targetEncodingName)
        {
            charsetDetector.Reset();
            // Detect the encoding format
            charsetDetector.Feed(inputStream);
            charsetDetector.DataEnd();

            // If no encoding format is detected, return an empty MemoryStream
            if (string.IsNullOrEmpty(charsetDetector.Charset) || string.IsNullOrEmpty(targetEncodingName))
            {
                return new MemoryStream(); // Return an empty Stream
            }

            // Reset the stream position
            inputStream.Position = 0;

            // Create a MemoryStream to store the converted data
            MemoryStream outputStream = new MemoryStream();

            try
            {
                // Convert the encoding and write to the MemoryStream
                charsetConverter.ConvertEncoding(inputStream, outputStream, charsetDetector.Charset, targetEncodingName);

                // Reset the MemoryStream position to the beginning so the caller can read it directly
                outputStream.Position = 0;
            }
            catch
            {
                // If an exception occurs during conversion, return an empty MemoryStream
                outputStream.Dispose(); // Ensure resources are released
                return new MemoryStream();
            }

            return outputStream;
        }
        public Stream DetectAndConvert(Stream inputStream, TargetEncoding targetEncoding)
        {
            string targetEncodingName = getEncodingName(targetEncoding);
            if(string.IsNullOrEmpty(targetEncodingName))
            {
                return new MemoryStream();
            }
            return DetectAndConvert(inputStream, targetEncodingName);
        }
        public string[,] DetectAndConvert(string[,] strings, TargetEncoding targetEncoding)
        {
            string targetEncodingName = getEncodingName(targetEncoding);
            if (string.IsNullOrEmpty(targetEncodingName))
            {
                return new string[0,0];
            }
            charsetDetector.Reset();
            charsetDetector.Feed(strings);
            charsetDetector.DataEnd();
            if (string.IsNullOrEmpty(charsetDetector.Charset) || string.IsNullOrEmpty(targetEncodingName))
            {
                return new string[0, 0]; 
            }
            try
            {
              return charsetConverter.ConvertEncoding(strings, charsetDetector.Charset, targetEncodingName);
            }
            catch
            {
                return new string[0, 0];
            }

        }
        public string[] DetectAndConvert(string[] strings, TargetEncoding targetEncoding)
        {
            string targetEncodingName = getEncodingName(targetEncoding);
            if (string.IsNullOrEmpty(targetEncodingName))
            {
                return new string[0];
            }
            charsetDetector.Reset();
            charsetDetector.Feed(strings);
            charsetDetector.DataEnd();
            if (string.IsNullOrEmpty(charsetDetector.Charset) || string.IsNullOrEmpty(targetEncodingName))
            {
                return new string[0];
            }
            try
            {
                return charsetConverter.ConvertEncoding(strings, charsetDetector.Charset, targetEncodingName);
            }
            catch
            {
                return new string[0];
            }

        }

        public string DetectAndConvert(string inputString, TargetEncoding targetEncoding)
        {
            string targetEncodingName = getEncodingName(targetEncoding);
            if (string.IsNullOrEmpty(targetEncodingName))
            {
                return string.Empty;
            }
            charsetDetector.Reset();
            charsetDetector.Feed(inputString);
            charsetDetector.DataEnd();
            if (string.IsNullOrEmpty(charsetDetector.Charset) || string.IsNullOrEmpty(targetEncodingName))
            {
                return  string.Empty; 
            }
            try
            {
                return charsetConverter.ConvertEncoding(inputString, charsetDetector.Charset, targetEncodingName);
            }
            catch
            {
                return string.Empty;
            }

        }
        /// <summary>
        /// Converts the encoding of a file.
        /// </summary>
        /// <param name="inputFilePath">The path to the input file</param>
        /// <param name="outputFilePath">The path to the output file</param>
        /// <param name="targetEncodingName">The name of the target encoding (e.g., "UTF-8")</param>
        public bool ConvertFileEncoding(string inputFilePath, string outputFilePath, string targetEncodingName)
        {
            try
            {
                // Open the input file stream
                using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    // Detect and convert the encoding
                    using (Stream convertedStream = DetectAndConvert(inputStream, targetEncodingName))
                    {
                        // If DetectAndConvert returns an empty Stream, it means detection failed or conversion is not supported
                        if (convertedStream.Length == 0)
                        {
                            Console.WriteLine("No supported encoding format detected, skipping conversion.");
                            return false;
                        }
                        // Write the converted Stream to the output file
                        using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        {
                            convertedStream.CopyTo(outputStream);
                        }
                    }
                }
                Console.WriteLine("File encoding conversion completed!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File encoding conversion failed: {ex.Message}");
                return false;
            }
        }
        public bool ConvertFileEncoding(string inputFilePath, string outputFilePath, TargetEncoding targetEncoding)
        {
            string targetEncodingName = getEncodingName(targetEncoding);
            if(string.IsNullOrEmpty(targetEncodingName))
            {
                return false;
            }
           return ConvertFileEncoding(inputFilePath, outputFilePath, targetEncodingName);
        }
        public string GetCharset(Stream inputStream)
        {
            charsetDetector.Reset();
            charsetDetector.Feed(inputStream);
            charsetDetector.DataEnd();
            return charsetDetector.Charset;
        }

        public string GetCharset(string[,] strings)
        {
            charsetDetector.Reset();
            charsetDetector.Feed(strings);
            charsetDetector.DataEnd();
            return charsetDetector.Charset;
        }

        public string GetCharset(string[] strings)
        {
            charsetDetector.Reset();
            charsetDetector.Feed(strings);
            charsetDetector.DataEnd();
            return charsetDetector.Charset;
        }

        public string GetCharset(string inputString)
        {
            charsetDetector.Reset();
            charsetDetector.Feed(inputString);
            charsetDetector.DataEnd();
            return charsetDetector.Charset;
        }

        private string getEncodingName(TargetEncoding targetEncoding)
        {
            switch (targetEncoding)
            {
                case TargetEncoding.ASCII:
                    return "ASCII";
                case TargetEncoding.UTF_8:
                    return "UTF-8";
                case TargetEncoding.UTF_16LE:
                    return "UTF-16LE";
                case TargetEncoding.UTF_16BE:
                    return "UTF-16BE";
                case TargetEncoding.UTF_32BE:
                    return "UTF-32BE";
                case TargetEncoding.UTF_32LE:
                    return "UTF-32LE";
                case TargetEncoding.windows_1251:
                    return "windows-1251";
                case TargetEncoding.windows_1252:
                    return "windows-1252";
                case TargetEncoding.windows_1253:
                    return "windows-1253";
                case TargetEncoding.windows_1255:
                    return "windows-1255";
                case TargetEncoding.Big5:
                    return "Big5";
                case TargetEncoding.EUC_KR:
                    return "EUC-KR";
                case TargetEncoding.EUC_JP:
                    return "EUC-JP";
                case TargetEncoding.ISO_2022_JP:
                    return "ISO-2022-JP";
                case TargetEncoding.ISO_2022_CN:
                    return "ISO-2022-CN";
                case TargetEncoding.ISO_2022_KR:
                    return "ISO-2022-KR";
                case TargetEncoding.HZ_GB_2312:
                    return "HZ-GB-2312";
                case TargetEncoding.Shift_JIS:
                    return "Shift-JIS";
                case TargetEncoding.x_mac_cyrillic:
                    return "x-mac-cyrillic";
                case TargetEncoding.KOI8_R:
                    return "KOI8-R";
                case TargetEncoding.IBM855:
                    return "IBM855";
                case TargetEncoding.IBM866:
                    return "IBM866";
                case TargetEncoding.ISO_8859_2:
                    return "ISO-8859-2";
                case TargetEncoding.ISO_8859_5:
                    return "ISO-8859-5";
                case TargetEncoding.ISO_8859_7:
                    return "ISO-8859-7";
                case TargetEncoding.ISO_8859_8:
                    return "ISO-8859-8";
                case TargetEncoding.GBK:
                    return "GBK";
                case TargetEncoding.GB2312:
                    return "GB2312";
                case TargetEncoding.GB18030:
                    return "GB18030";
                default:
                    return string.Empty;
            }
        }
    }

}

