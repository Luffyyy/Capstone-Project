using System;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

[Serializable]
public class TelevisionChannel
{
    public int Number;
    public GameObject Content;
}

public class Television : Entity
{
    [SyncVar(hook=nameof(OnChannelNumberChanged))]
    public int CurrentChannelNumber;

    public TextMeshProUGUI ChannelIndicator;

    private GameObject currentChannel;

    public List<TelevisionChannel> Channels;

    void Awake()
    {
        SetChannel(CurrentChannelNumber);
    }

    public void OnChannelNumberChanged(int oldChannelNumber, int newChannelNumber)
    {
        SetChannel(newChannelNumber);
    }

    public void SetChannel(int num)
    {
        if (num < 0 || num > 999)
        {
            return;
        }

        var channelObject = Channels.Find(ch => ch.Number == num);

        CurrentChannelNumber = num;

        ChannelIndicator.SetText("Channel: "+num);

        if (currentChannel != null)
        {
            currentChannel.SetActive(false);
        }

        if (channelObject != null)
        {
            currentChannel = channelObject.Content;
            currentChannel.SetActive(true);
        }
    }
}
