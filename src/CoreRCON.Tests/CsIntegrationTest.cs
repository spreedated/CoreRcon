﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRCON.PacketFormats;
using Xunit;
using Xunit.Abstractions;


namespace CoreRCON.Tests
{

    [Trait("Type", "Integration")]
    public class CsIntegrationTest(CsServerFixture serverFixture, ITestOutputHelper output) : IClassFixture<CsServerFixture>
    {
        [Fact]
        public async Task ConnectShouldConnectAndAuthenticate()
        {
            using Rcon rcon = serverFixture.GetRconClient();
            await rcon.ConnectAsync();

            Assert.True(rcon.Connected);
            Assert.True(rcon.Authenticated);
            output.WriteLine("Rcon connection to server successfull");
        }

        [Fact]
        public async Task EchoCommandShouldReturnInputedText()
        {
            using Rcon rconClient = serverFixture.GetRconClient();
            await rconClient.ConnectAsync();

            string response = await rconClient.SendCommandAsync("echo hi");
            Assert.Contains("hi", response);
        }

        [Fact]
        public async Task TestConnectWithWrongPasswordShouldThrowAuthenticationException()
        {
            //Warning ! This test can ban your ip in the server if sv_rcon_maxfailure is set to 0
            //Use removeip to unban your ip (Default ban period is 60 min)
            Rcon rconClient = new(serverFixture._rconEndpoint, "wrong PW");
            await Assert.ThrowsAsync<AuthenticationException>(rconClient.ConnectAsync);
            Assert.True(rconClient.Connected);
            Assert.False(rconClient.Authenticated);
        }

        [Fact]
        public async Task TestCommentShouldReturnEmptyResponse()
        {
            using Rcon rconClient = serverFixture.GetRconClient();
            await rconClient.ConnectAsync();

            string response = await rconClient.SendCommandAsync("//comment");
            Assert.Equal("", response);
        }

        [Fact]
        public async Task TestCvarListShouldReturnWholeResponse()
        {
            using Rcon rconClient = serverFixture.GetRconClient();
            await rconClient.ConnectAsync();

            string response = await rconClient.SendCommandAsync("cvarlist");
            Assert.EndsWith("total convars/concommands", response);
        }


        [Fact]
        public async Task TestMultipleSyncronousCommandsShouldReturn()
        {
            using Rcon rconClient = serverFixture.GetRconClient();
            await rconClient.ConnectAsync();

            for (int i = 0; i < 10; i++)
            {
                string response = await rconClient.SendCommandAsync($"echo {i}");
                Assert.Contains($"{i}", response);
            }
        }

        [Fact]
        public async Task TestMultipleAsyncronousCommandsShouldReturn()
        {
            List<Task> tasks = [];

            using Rcon rconClient = serverFixture.GetRconClient();
            await rconClient.ConnectAsync();

            tasks = Enumerable.Range(1, 10)
                .Select(async (i) =>
                {
                    string response = await rconClient.SendCommandAsync($"echo {i}");
                    Assert.Contains($"{i}", response);
                }).ToList();
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task InfoQueryShouldReturnValidPayload()
        {
            SourceQueryInfo result = (SourceQueryInfo)await ServerQuery.Info(serverFixture._rconEndpoint, ServerQuery.ServerType.Source);
            Assert.NotNull(result);
            Assert.NotNull(result.Name);
        }

        [Fact]
        public async Task PlayerQueryShouldReturnValidPayload()
        {
            ServerQueryPlayer[] result = await ServerQuery.Players(serverFixture._rconEndpoint);
            Assert.NotNull(result);
        }
    }
}
