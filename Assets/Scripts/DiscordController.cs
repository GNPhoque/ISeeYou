using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    public Discord.Discord discord;

    void Start()
    {
        discord = new Discord.Discord(922533722841047050, (System.UInt64)Discord.CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity()
        {
            State = "Playing I See You (Unity Dev)"
        };
        activityManager.UpdateActivity(activity, res =>
        {
            if (res == Result.Ok)
            {
                Debug.Log("Discord Status Set!");
            }
            else
            {
                Debug.Log("Discord Status Failed...");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }

	private void OnApplicationQuit()
	{
        discord.GetActivityManager().ClearActivity((res) => Debug.Log(res.ToString()));
    }
}
