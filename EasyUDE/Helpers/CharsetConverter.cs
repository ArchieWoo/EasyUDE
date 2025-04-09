using System.Text;

namespace EasyUDE.Helpers
{
    public class CharsetConverter
    {
        public CharsetConverter()
        {
            try
            {
                // Register the CodePagesEncodingProvider to support additional encodings like GB18030
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to register encoding provider: {ex.Message}");
            }
        }

        /// <summary>
        /// Converts the input stream from the source encoding to the target encoding.
        /// </summary>
        /// <param name="inputStream">The original data stream</param>
        /// <param name="sourceEncodingName">The name of the source encoding (e.g., "UTF-8", "Shift-JIS")</param>
        /// <param name="targetEncodingName">The name of the target encoding</param>
        /// <returns>The converted byte array</returns>
        public byte[] ConvertEncoding(Stream inputStream, string sourceEncodingName, string targetEncodingName)
        {
            try
            {
                // Get the source and target encoding objects
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding targetEncoding = Encoding.GetEncoding(targetEncodingName);

                //// Read the input stream and decode it into a string
                //using (StreamReader reader = new StreamReader(inputStream, sourceEncoding))
                //{
                //    string decodedString = reader.ReadToEnd();

                //    // Re-encode the string to the target encoding
                //    return targetEncoding.GetBytes(decodedString);
                //}
                if (inputStream.CanSeek)
                {
                    inputStream.Position = 0;
                }
                // Read the input stream and decode it into a string
                using (StreamReader reader = new StreamReader(inputStream, sourceEncoding))
                {
                    string decodedString = reader.ReadToEnd();

                    // Re-encode the string to the target encoding
                    byte[] encodedBytes = targetEncoding.GetBytes(decodedString);

                    // Add BOM if the target encoding requires it
                    if (targetEncoding.CodePage == Encoding.UTF8.CodePage ||
                        targetEncoding.CodePage == Encoding.Unicode.CodePage ||
                        targetEncoding.CodePage == Encoding.BigEndianUnicode.CodePage)
                    {
                        byte[] bom = targetEncoding.GetPreamble();
                        byte[] result = new byte[bom.Length + encodedBytes.Length];
                        Array.Copy(bom, 0, result, 0, bom.Length);
                        Array.Copy(encodedBytes, 0, result, bom.Length, encodedBytes.Length);
                        return result;
                    }

                    return encodedBytes;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during encoding conversion: {ex.Message}");
                return Array.Empty<byte>(); // Return an empty byte array in case of failure
            }
        }
        public string[,] ConvertEncoding(string[,] strings, string sourceEncodingName, string targetEncodingName)
        {
            if (strings == null)
            {
                Console.WriteLine($"Input array is null");
                return new string[0, 0];
            }
            // Get the dimensions of the input array
            int rows = strings.GetLength(0);
            int cols = strings.GetLength(1);
            string[,] outputArray = new string[rows, cols];
            try
            {
                // Get the source and target encoding objects
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding targetEncoding = Encoding.GetEncoding(targetEncodingName);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        string originalString = strings[i, j];
                        // Skip empty or null cells
                        if (string.IsNullOrEmpty(originalString))
                        {
                            outputArray[i, j] = originalString;
                            continue;
                        }
                        try
                        {
                            // Convert the string from source encoding to target encoding
                            byte[] sourceBytes = sourceEncoding.GetBytes(originalString);
                            string convertedString = targetEncoding.GetString(sourceBytes);
                            outputArray[i, j] = convertedString;
                        }
                        catch (Exception cellEx)
                        {
                            // Log the error and preserve the original string
                            Console.WriteLine($"Error converting cell [{i}, {j}]: {cellEx.Message}");
                            outputArray[i, j] = originalString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle global errors (e.g., invalid encoding names)
                Console.WriteLine($"Global error during encoding conversion: {ex.Message}");
                return strings; // Return the original array in case of failure
            }
            return outputArray;
        }
        /// <summary>
        /// Converts the encoding of each string in a 1D string array from the source encoding to the target encoding.
        /// </summary>
        /// <param name="strings">The input 1D string array</param>
        /// <param name="sourceEncodingName">The name of the source encoding (e.g., "UTF-8", "GB18030")</param>
        /// <param name="targetEncodingName">The name of the target encoding (e.g., "UTF-8", "ASCII")</param>
        /// <returns>A new 1D string array with converted content</returns>
        public string[] ConvertEncoding(string[] strings, string sourceEncodingName, string targetEncodingName)
        {
            // If the input array is null, return an empty array
            if (strings == null)
            {
                return Array.Empty<string>();
            }

            // Get the length of the input array
            int length = strings.Length;

            // Create a new array to store the converted strings
            string[] outputArray = new string[length];

            try
            {
                // Get the source and target encoding objects
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding targetEncoding = Encoding.GetEncoding(targetEncodingName);

                // Iterate through each string in the input array
                for (int i = 0; i < length; i++)
                {
                    string originalString = strings[i];

                    // Skip empty or null strings
                    if (string.IsNullOrEmpty(originalString))
                    {
                        outputArray[i] = originalString;
                        continue;
                    }

                    try
                    {
                        // Convert the string from source encoding to target encoding
                        byte[] sourceBytes = sourceEncoding.GetBytes(originalString);
                        string convertedString = targetEncoding.GetString(sourceBytes);

                        // Store the converted string in the output array
                        outputArray[i] = convertedString;
                    }
                    catch (Exception cellEx)
                    {
                        // Log the error and preserve the original string
                        Console.WriteLine($"Error converting string at index [{i}]: {cellEx.Message}");
                        outputArray[i] = originalString;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle global errors (e.g., invalid encoding names)
                Console.WriteLine($"Global error during encoding conversion: {ex.Message}");

                // Return the original array in case of failure
                return strings;
            }

            // Return the converted 1D string array
            return outputArray;
        }
        /// <summary>
        /// Converts the encoding of a single string from the source encoding to the target encoding.
        /// </summary>
        /// <param name="inputString">The input string to be converted</param>
        /// <param name="sourceEncodingName">The name of the source encoding (e.g., "UTF-8", "GB18030")</param>
        /// <param name="targetEncodingName">The name of the target encoding (e.g., "UTF-8", "ASCII")</param>
        /// <returns>The converted string in the target encoding</returns>
        public string ConvertEncoding(string inputString, string sourceEncodingName, string targetEncodingName)
        {
            // If the input string is null or empty, return it as-is
            if (string.IsNullOrEmpty(inputString))
            {
                return inputString;
            }

            try
            {
                // Get the source and target encoding objects
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding targetEncoding = Encoding.GetEncoding(targetEncodingName);

                // Convert the string from source encoding to target encoding
                byte[] sourceBytes = sourceEncoding.GetBytes(inputString);
                string convertedString = targetEncoding.GetString(sourceBytes);

                return convertedString;
            }
            catch (Exception ex)
            {
                // Log the error and return the original string
                Console.WriteLine($"Error converting string: {ex.Message}");
                return inputString;
            }
        }
        /// <summary>
        /// Converts the input stream from the source encoding to the target encoding and writes it to the output stream.
        /// </summary>
        /// <param name="inputStream">The original data stream</param>
        /// <param name="outputStream">The output data stream</param>
        /// <param name="sourceEncodingName">The name of the source encoding</param>
        /// <param name="targetEncodingName">The name of the target encoding</param>
        public void ConvertEncoding(Stream inputStream, Stream outputStream, string sourceEncodingName, string targetEncodingName)
        {
            try
            {
                // Convert the input stream to the target encoding
                byte[] convertedBytes = ConvertEncoding(inputStream, sourceEncodingName, targetEncodingName);

                // Write the converted bytes to the output stream
                outputStream.Write(convertedBytes, 0, convertedBytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during stream encoding conversion: {ex.Message}");
            }
        }

    }
}