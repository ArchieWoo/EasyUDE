namespace EasyUDE
{
    /// <summary>
    /// Interface for handling character encoding detection and conversion.
    /// Provides methods to detect and convert encodings for various types of input data, including streams, 2D string arrays, 1D string arrays, individual strings, and files.
    /// </summary>
    public interface ICharsetHandler
    {
        /// <summary>
        /// Detects the character encoding of the given input stream.
        /// </summary>
        /// <param name="inputStream">The input stream containing the data to analyze.</param>
        /// <returns>The detected character encoding as a string (e.g., "UTF-8", "ISO-8859-1"). 
        /// If detection fails, returns null or an empty string.</returns>
        string GetCharset(Stream inputStream);
        /// <summary>
        /// Detects the character encoding of the given 2D string array.
        /// </summary>
        /// <param name="strings">A 2D string array containing the data to analyze.</param>
        /// <returns>The detected character encoding as a string (e.g., "UTF-8", "ISO-8859-1"). 
        /// If detection fails, returns null or an empty string.</returns>
        string GetCharset(string[,] strings);
        /// <summary>
        /// Detects the character encoding of the given 1D string array.
        /// </summary>
        /// <param name="strings">A 1D string array containing the data to analyze.</param>
        /// <returns>The detected character encoding as a string (e.g., "UTF-8", "ISO-8859-1"). 
        /// If detection fails, returns null or an empty string.</returns>
        string GetCharset(string[] strings);
        /// <summary>
        /// Detects the character encoding of the given single string.
        /// </summary>
        /// <param name="inputString">The input string to analyze.</param>
        /// <returns>The detected character encoding as a string (e.g., "UTF-8", "ISO-8859-1"). 
        /// If detection fails, returns null or an empty string.</returns>
        string GetCharset(string inputString);

        /// <summary>
        /// Detects the encoding format of the input stream and converts it to the target encoding.
        /// </summary>
        /// <param name="inputStream">The input stream</param>
        /// <param name="targetEncodingName">The name of the target encoding</param>
        /// <returns>A Stream containing the converted data (if no encoding format is detected, returns an empty Stream)</returns>
        Stream DetectAndConvert(Stream inputStream, string targetEncodingName);

        /// <summary>
        /// Detects the encoding of the input stream and converts it to the specified target encoding.
        /// </summary>
        /// <param name="inputStream">The input stream containing the data to be analyzed and converted.</param>
        /// <param name="targetEncoding">The target encoding format to which the data should be converted.</param>
        /// <returns>A Stream object containing the converted data. If detection fails, an empty or unmodified stream may be returned.</returns>
        Stream DetectAndConvert(Stream inputStream, TargetEncoding targetEncoding);

        /// <summary>
        /// Detects the encoding of a 2D string array and converts all its elements to the specified target encoding.
        /// </summary>
        /// <param name="strings">A 2D string array containing the data to be analyzed and converted.</param>
        /// <param name="targetEncoding">The target encoding format to which the data should be converted.</param>
        /// <returns>A new 2D string array with all elements converted to the target encoding. If detection fails, the original array may be returned.</returns>
        string[,] DetectAndConvert(string[,] strings, TargetEncoding targetEncoding);

        /// <summary>
        /// Detects the encoding of a 1D string array and converts all its elements to the specified target encoding.
        /// </summary>
        /// <param name="strings">A 1D string array containing the data to be analyzed and converted.</param>
        /// <param name="targetEncoding">The target encoding format to which the data should be converted.</param>
        /// <returns>A new 1D string array with all elements converted to the target encoding. If detection fails, the original array may be returned.</returns>
        string[] DetectAndConvert(string[] strings, TargetEncoding targetEncoding);

        /// <summary>
        /// Detects the encoding of a single string and converts it to the specified target encoding.
        /// </summary>
        /// <param name="inputString">The input string to be analyzed and converted.</param>
        /// <param name="targetEncoding">The target encoding format to which the string should be converted.</param>
        /// <returns>A new string converted to the target encoding. If detection fails, the original string may be returned.</returns>
        string DetectAndConvert(string inputString, TargetEncoding targetEncoding);

        /// <summary>
        /// Converts the encoding of a file from its detected encoding to the specified target encoding.
        /// </summary>
        /// <param name="inputFilePath">The path to the input file whose encoding needs to be converted.</param>
        /// <param name="outputFilePath">The path to the output file where the converted content will be saved.</param>
        /// <param name="targetEncoding">The target encoding format to which the file content should be converted.</param>
        /// <returns>True if the file encoding conversion was successful; otherwise, false.</returns>
        bool ConvertFileEncoding(string inputFilePath, string outputFilePath, TargetEncoding targetEncoding);
        
        /// <summary>
        /// Converts the encoding of a file.
        /// </summary>
        /// <param name="inputFilePath">The path to the input file</param>
        /// <param name="outputFilePath">The path to the output file</param>
        /// <param name="targetEncodingName">The name of the target encoding (e.g., "UTF-8")</param>
        bool ConvertFileEncoding(string inputFilePath, string outputFilePath, string targetEncodingName);
    }
}