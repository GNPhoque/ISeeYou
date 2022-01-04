using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;

public class RelayManager : NetworkBehaviour
{
	[SerializeField]
	TMP_Text joinCodeText;
	[SerializeField]
	string environment = "production";
	[SerializeField]
	int maxConnections = 2;

	public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

	public UnityTransport Transport => NetworkManager.gameObject.GetComponent<UnityTransport>();

	public async Task<RelayHostData> SetupRelay()
	{
		InitializationOptions options = new InitializationOptions().SetEnvironmentName("production");

		await UnityServices.InitializeAsync(options);

		if (!AuthenticationService.Instance.IsSignedIn)
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}

		Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);
		RelayHostData relayHostData = new RelayHostData
		{
			Key = allocation.Key,
			Port = (ushort)allocation.RelayServer.Port,
			AllocationID = allocation.AllocationId,
			AllocationIDBytes = allocation.AllocationIdBytes,
			IPv4Address = allocation.RelayServer.IpV4,
			ConnectionData = allocation.ConnectionData
		};

		relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);
		joinCodeText.text = $"Join code : {relayHostData.JoinCode}";

		Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes, relayHostData.Key, relayHostData.ConnectionData);

		return relayHostData;
	} 

	public async Task<RelayJoinData> JoinRelay(string joinCode)
	{
		InitializationOptions options = new InitializationOptions().SetEnvironmentName("production");

		await UnityServices.InitializeAsync(options);

		if (!AuthenticationService.Instance.IsSignedIn)
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}

		JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

		RelayJoinData relayJoinData = new RelayJoinData
		{
			Key = allocation.Key,
			Port = (ushort)allocation.RelayServer.Port,
			AllocationID = allocation.AllocationId,
			AllocationIDBytes = allocation.AllocationIdBytes,
			ConnectionData = allocation.ConnectionData,
			HostConnectionData = allocation.HostConnectionData,
			IPv4Address = allocation.RelayServer.IpV4,
			JoinCode = joinCode
		};
		joinCodeText.text = $"Join code : {joinCode}";

		Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port, relayJoinData.AllocationIDBytes, relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);

		return relayJoinData;
	}
}
