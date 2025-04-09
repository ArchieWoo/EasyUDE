using System.Text;

namespace EasyUDE.Test
{
    [TestClass]
    public class GB18030Test
    {
        [TestMethod]
        public void SimpleTest1()
        {
            // 初始化 CharsetDetector 实例
            CharsetHandler detector = new CharsetHandler();

            // 定义输入和输出文件路径
            string filePath = @"..\..\..\Data\TestFile_GB18030.txt";
            string outputFilePath = @"..\..\..\Data\OutputFile_GB18030_2_UTF_8.txt";

            try
            {
                // 打开输入文件流
                using (FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 检测并转换编码
                    using (Stream convertedStream = detector.DetectAndConvert(inputStream, "UTF-8"))
                    {
                        // 将转换后的 Stream 写入输出文件
                        using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        {
                            convertedStream.CopyTo(outputStream);
                        }
                    }
                }

                // 验证输出文件是否存在
                Assert.IsTrue(File.Exists(outputFilePath), "输出文件未生成！");

                // 验证输出文件内容是否正确
                string expectedContent = File.ReadAllText(filePath, Encoding.GetEncoding("gb18030"));
                string actualContent = File.ReadAllText(outputFilePath, Encoding.UTF8);

                Assert.AreEqual(expectedContent, actualContent, "转换后的文件内容与原始内容不一致！");
            }
            catch (Exception ex)
            {
                Assert.Fail($"测试失败: {ex.Message}");
            }

        }
        [TestMethod]
        public void ConvertFileEncodingTest()
        {
            // 定义输入和输出文件路径
            string filePath = @"..\..\..\Data\TestFile_Big5.txt";
            string outputFilePath = @"..\..\..\Data\OutputFile_Big5_2_UTF8.txt";
            CharsetHandler detector = new CharsetHandler();
            detector.ConvertFileEncoding(filePath, outputFilePath, "UTF-8");

            Assert.IsTrue(File.Exists(outputFilePath), "输出文件未生成！");

            // 验证输出文件内容是否正确
            string expectedContent = File.ReadAllText(filePath, Encoding.GetEncoding("Big5"));
            string actualContent = File.ReadAllText(outputFilePath, Encoding.GetEncoding("UTF-8"));
            Assert.AreEqual(expectedContent, actualContent, "转换后的文件内容与原始内容不一致！");
        }
    }
}
