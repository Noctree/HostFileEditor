using NUnit.Framework;
using Host_File_Editor;

namespace Tests
{
    public class StringHelperTests
    {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void GetFirstWord() {
            Assert.AreEqual("first", StringHelper.TakeWord("first second", 0));
        }

        [Test]
        public void GetFirstWord_OnlyWord() {
            Assert.AreEqual("first", StringHelper.TakeWord("first", 0));
        }

        [Test]
        public void GetFirstWord_WhitespacePadding() {
            Assert.AreEqual("first", StringHelper.TakeWord("   first second", 0));
        }

        [Test]
        public void GetSecondWord() {
            Assert.AreEqual("second", StringHelper.TakeWord("first second", 1));
        }

        [Test]
        public void GetSecondWord_WhitespacePadding() {
            Assert.AreEqual("second", StringHelper.TakeWord("   first   second", 1));
        }

        [Test]
        public void GetSecondWord_TabPadding() {
            Assert.AreEqual("second", StringHelper.TakeWord("first      second", 1));
        }

        //========================================================================================
        //
        // Test entries are from Dan Pollock's hostfile (https://someonewhocares.org/hosts/hosts)
        //
        //========================================================================================

        [Test]
        public void HostFile_GetDestination() {
            string entry = "127.0.0.1 123counter.mycomputer.com";
            Assert.AreEqual("127.0.0.1", StringHelper.TakeWord(entry, 0));
        }

        [Test]
        public void HostFile_GetSource() {
            string entry = "127.0.0.1 123counter.mycomputer.com";
            Assert.AreEqual("123counter.mycomputer.com", StringHelper.TakeWord(entry, 1));
        }

        [Test]
        public void HostFile_WithComment_GetSource() {
            string entry = "127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("media.fastclick.net", StringHelper.TakeWord(entry, 1));
        }

        [Test]
        public void HostFile_WithComment_GetCommentSymbol() {
            string entry = "127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("#", StringHelper.TakeWord(entry, 2));
        }

        [Test]
        public void HostFile_WithComment_GetCommentFirstWord() {
            string entry = "127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("Likewise,", StringHelper.TakeWord(entry, 3));
        }

        [Test]
        public void HostFile_CommentedOutEntry_WithComment_GetSource() {
            string entry = "#127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("#127.0.0.1", StringHelper.TakeWord(entry, 0));
        }

        [Test]
        public void HostFile_CommentedOutEntry_WithComment_GetSource_WhitespacePadding() {
            string entry = "      #127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("#127.0.0.1", StringHelper.TakeWord(entry, 0));
        }

        [Test]
        public void HostFile_CommentedOutEntry_WithComment_GetSource_TabPadding() {
            string entry = "            #127.0.0.1 media.fastclick.net	# Likewise, this may interfere with some";
            Assert.AreEqual("#127.0.0.1", StringHelper.TakeWord(entry, 0));
        }
    }
}