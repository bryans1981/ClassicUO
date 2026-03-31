using System;
using System.IO;
using ClassicUO.Utility.Platforms;
using FluentAssertions;
using Xunit;

namespace ClassicUO.UnitTests.Utility.FileSystemHelper
{
    public class BrowserFileSystemTests
    {
        [Fact]
        public void NormalizePath_Should_Canonicalize_Windows_Style_And_Dot_Segments()
        {
            string normalized = BrowserFileSystem.NormalizePath(@"assets\..\Data\.\tiledata.mul");

            normalized.Should().Be("/Data/tiledata.mul");
        }

        [Fact]
        public void InMemoryProvider_Should_Be_CaseInsensitive_And_Use_Normalized_Paths()
        {
            var provider = new InMemoryBrowserStorageProvider();

            provider.WriteAllText(@"assets\Config\Login.json", "hello");

            provider.FileExists("/ASSETS/config/login.json").Should().BeTrue();
            provider.DirectoryExists(@"\assets\config").Should().BeTrue();
            provider.ReadAllText("/assets/config/login.json").Should().Be("hello");
        }

        [Fact]
        public void InMemoryProvider_GetFiles_Should_Return_Only_Direct_Children()
        {
            var provider = new InMemoryBrowserStorageProvider();

            provider.WriteAllText("/assets/a.txt", "a");
            provider.WriteAllText("/assets/nested/b.txt", "b");
            provider.WriteAllText("/assets/c.txt", "c");

            provider.GetFiles("/assets").Should().BeEquivalentTo(new[]
            {
                "/assets/a.txt",
                "/assets/c.txt"
            });
        }

        [Fact]
        public void BrowserFileSystem_Should_Throw_When_No_Provider_Is_Registered()
        {
            BrowserFileSystem.SetProvider(null);
            var fileSystem = new BrowserFileSystem();

            Action act = () => fileSystem.OpenRead("/assets/a.txt");

            act.Should().Throw<PlatformNotSupportedException>();
        }

        [Fact]
        public void BrowserFileSystem_Should_Delegate_To_Provider_With_Normalized_Path()
        {
            var provider = new InMemoryBrowserStorageProvider();
            var fileSystem = new BrowserFileSystem();

            BrowserFileSystem.SetProvider(provider);
            provider.WriteAllText("/assets/config.json", "value");

            using Stream stream = fileSystem.OpenRead(@"assets\config.json");
            using StreamReader reader = new StreamReader(stream);

            reader.ReadToEnd().Should().Be("value");

            BrowserFileSystem.SetProvider(null);
        }

        [Fact]
        public void InMemoryProvider_OpenWrite_Should_Persist_On_Dispose()
        {
            var provider = new InMemoryBrowserStorageProvider();

            using (Stream stream = provider.OpenWrite("/assets/profile.json"))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("saved");
            }

            provider.ReadAllText("/assets/profile.json").Should().Be("saved");
        }

        [Fact]
        public void InMemoryProvider_OpenAppend_Should_Append_To_Existing_Content()
        {
            var provider = new InMemoryBrowserStorageProvider();
            provider.WriteAllText("/assets/profile.json", "saved");

            using (Stream stream = provider.OpenAppend("/assets/profile.json"))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("-again");
            }

            provider.ReadAllText("/assets/profile.json").Should().Be("saved-again");
        }

        [Fact]
        public void InMemoryProvider_GetFiles_With_SearchPattern_Should_Filter_By_Extension()
        {
            var provider = new InMemoryBrowserStorageProvider();
            provider.WriteAllText("/assets/a.png", "a");
            provider.WriteAllText("/assets/b.jpg", "b");
            provider.WriteAllText("/assets/c.png", "c");

            provider.GetFiles("/assets", "*.png").Should().BeEquivalentTo(new[]
            {
                "/assets/a.png",
                "/assets/c.png"
            });
        }

        [Fact]
        public void InMemoryProvider_OpenReadWrite_Should_Allow_Overwrite_And_Report_Length()
        {
            var provider = new InMemoryBrowserStorageProvider();
            provider.WriteAllText("/assets/map0.mul", "abcd");

            using (Stream stream = provider.OpenReadWrite("/assets/map0.mul"))
            {
                stream.SetLength(0);

                using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
                writer.Write("xy");
                writer.Flush();
            }

            provider.ReadAllText("/assets/map0.mul").Should().Be("xy");
            provider.GetFileLength("/assets/map0.mul").Should().Be(2);
        }

        [Fact]
        public void InMemoryProvider_CopyFile_Should_Copy_Contents()
        {
            var provider = new InMemoryBrowserStorageProvider();
            provider.WriteAllText("/assets/a.mul", "copy-me");

            provider.CopyFile("/assets/a.mul", "/assets/b.mul", overwrite: true);

            provider.ReadAllText("/assets/b.mul").Should().Be("copy-me");
        }

        [Fact]
        public void InMemoryProvider_DeleteFile_Should_Remove_File()
        {
            var provider = new InMemoryBrowserStorageProvider();
            provider.WriteAllText("/assets/delete.me", "x");

            provider.DeleteFile("/assets/delete.me");

            provider.FileExists("/assets/delete.me").Should().BeFalse();
        }

        [Fact]
        public void RootedProvider_Should_Route_Reads_To_Correct_Root_Backend()
        {
            var assets = new InMemoryBrowserStorageProvider();
            var profiles = new InMemoryBrowserStorageProvider();
            var cache = new InMemoryBrowserStorageProvider();
            var config = new InMemoryBrowserStorageProvider();
            var provider = new RootedBrowserStorageProvider(assets, profiles, cache, config);

            assets.WriteAllText("/map0.mul", "asset");
            profiles.WriteAllText("/player-one/macros.xml", "profile");

            provider.ReadAllText("/uo/map0.mul").Should().Be("asset");
            provider.ReadAllText("/profiles/player-one/macros.xml").Should().Be("profile");
        }

        [Fact]
        public void RootedProvider_Should_Prefix_Listed_Files_With_Full_Root_Path()
        {
            var assets = new InMemoryBrowserStorageProvider();
            var provider = new RootedBrowserStorageProvider(assets, new InMemoryBrowserStorageProvider(), new InMemoryBrowserStorageProvider(), new InMemoryBrowserStorageProvider());

            assets.WriteAllText("/TestDoc.txt", "hello");

            provider.GetFiles("/uo").Should().ContainSingle().Which.Should().Be("/uo/TestDoc.txt");
        }

        [Fact]
        public void RootedProvider_Should_Copy_Across_Roots()
        {
            var assets = new InMemoryBrowserStorageProvider();
            var profiles = new InMemoryBrowserStorageProvider();
            var provider = new RootedBrowserStorageProvider(assets, profiles, new InMemoryBrowserStorageProvider(), new InMemoryBrowserStorageProvider());

            assets.WriteAllText("/TestDoc.txt", "hello");

            provider.CopyFile("/uo/TestDoc.txt", "/profiles/player-one/TestDoc.txt", overwrite: true);

            profiles.ReadAllText("/player-one/TestDoc.txt").Should().Be("hello");
        }

        [Fact]
        public void RootedProvider_Should_Reject_Unknown_Roots()
        {
            var provider = new RootedBrowserStorageProvider
            (
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider()
            );

            Action act = () => provider.FileExists("/misc/readme.txt");

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void BinaryAssetProvider_Should_OpenRead_And_Report_Length()
        {
            var source = new InMemoryBrowserBinaryAssetSource();
            var provider = new BrowserBinaryAssetStorageProvider(source);
            source.AddFile("/tiledata.mul", new byte[] { 0, 1, 2, 3 });

            provider.FileExists("/tiledata.mul").Should().BeTrue();
            provider.GetFileLength("/tiledata.mul").Should().Be(4);

            using Stream stream = provider.OpenRead("/tiledata.mul");
            using MemoryStream copy = new MemoryStream();
            stream.CopyTo(copy);

            copy.ToArray().Should().Equal(new byte[] { 0, 1, 2, 3 });
        }

        [Fact]
        public void BinaryAssetProvider_Should_Be_ReadOnly()
        {
            var source = new InMemoryBrowserBinaryAssetSource();
            var provider = new BrowserBinaryAssetStorageProvider(source);

            Action act = () => provider.OpenWrite("/tiledata.mul");

            act.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void RootedProvider_Should_Support_ReadOnly_Asset_Backend_With_Writable_Profile_Backend()
        {
            var assets = new InMemoryBrowserBinaryAssetSource();
            var profiles = new InMemoryBrowserStorageProvider();
            var provider = new RootedBrowserStorageProvider
            (
                new BrowserBinaryAssetStorageProvider(assets),
                profiles,
                new InMemoryBrowserStorageProvider(),
                new InMemoryBrowserStorageProvider()
            );

            assets.AddFile("/tiledata.mul", new byte[] { 85, 78, 85, 83, 69, 68 });
            profiles.WriteAllText("/player-one/macros.xml", "<macros />");

            provider.FileExists("/uo/tiledata.mul").Should().BeTrue();
            provider.GetFileLength("/uo/tiledata.mul").Should().Be(6);
            provider.ReadAllText("/profiles/player-one/macros.xml").Should().Be("<macros />");
        }
    }
}
