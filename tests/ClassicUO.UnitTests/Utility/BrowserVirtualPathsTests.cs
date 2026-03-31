using ClassicUO.Utility;
using FluentAssertions;
using Xunit;

namespace ClassicUO.UnitTests.Utility
{
    public class BrowserVirtualPathsTests
    {
        [Fact]
        public void Normalize_Should_Canonicalize_Relative_Segments()
        {
            BrowserVirtualPaths.Normalize(@"\uo\art\..\map0.mul").Should().Be("/uo/map0.mul");
        }

        [Fact]
        public void Combine_Should_Create_Normalized_Rooted_Path()
        {
            BrowserVirtualPaths.Combine("/uo/", "maps", "map0.mul").Should().Be("/uo/maps/map0.mul");
        }

        [Fact]
        public void AssetFile_Should_Place_File_Under_Assets_Root()
        {
            BrowserVirtualPaths.AssetFile("Test1/New folder/file.mul").Should().Be("/uo/Test1/New folder/file.mul");
        }

        [Fact]
        public void ProfileFile_Should_Place_File_Under_Profile_Root()
        {
            BrowserVirtualPaths.ProfileFile("player-one", "macros.xml").Should().Be("/profiles/player-one/macros.xml");
        }

        [Fact]
        public void Classify_Should_Recognize_Asset_Paths()
        {
            BrowserVirtualPathInfo info = BrowserVirtualPaths.Classify("/uo/Test1/map0.mul");

            info.Root.Should().Be(BrowserVirtualRoot.Assets);
            info.RelativePath.Should().Be("Test1/map0.mul");
            info.IsKnownRoot.Should().BeTrue();
        }

        [Fact]
        public void Classify_Should_Recognize_Profile_Paths()
        {
            BrowserVirtualPathInfo info = BrowserVirtualPaths.Classify("/profiles/player-one/macros.xml");

            info.Root.Should().Be(BrowserVirtualRoot.Profiles);
            info.RelativePath.Should().Be("player-one/macros.xml");
            info.IsKnownRoot.Should().BeTrue();
        }

        [Fact]
        public void Classify_Should_Return_Unknown_For_Unmapped_Roots()
        {
            BrowserVirtualPathInfo info = BrowserVirtualPaths.Classify("/misc/readme.txt");

            info.Root.Should().Be(BrowserVirtualRoot.Unknown);
            info.RelativePath.Should().Be("misc/readme.txt");
            info.IsKnownRoot.Should().BeFalse();
        }

        [Fact]
        public void IsUnderRoot_Should_Match_Case_Insensitively()
        {
            BrowserVirtualPaths.IsUnderRoot("/UO/Maps/map0.mul", BrowserVirtualPaths.AssetsRoot).Should().BeTrue();
            BrowserVirtualPaths.IsUnderRoot("/misc/readme.txt", BrowserVirtualPaths.AssetsRoot).Should().BeFalse();
        }
    }
}
