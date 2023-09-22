﻿using System.Drawing;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Y2DL.Models;
using SmartFormat;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Settings;
using SmartFormat.Extensions;
using Y2DL.SmartFormatters;

namespace Y2DL.Utils;

public static class EmbedUtils
{
    public static Discord.Embed GenerateHelpEmbed()
    {
        EmbedBuilder embedBuilder = new EmbedBuilder()
        {
            Title = "Help",
            Description = "Please note that this bot uses only **slash commands**, but with exceptions:\r\n" +
                          "Using the YouTubeInfo system also works in guilds if you mention this bot."
        };

        embedBuilder.AddField("/about (y2dl|bot)", "About Y2DL or the bot", true);
        embedBuilder.AddField("/shards", "Get sharding information", true);
        embedBuilder.AddField("/ytinfo (channel|playlist|video)", "Get info for youtube channel, playlist, or video", true);

        return embedBuilder.Build();
    }
    
    public static EmbedBuilder ToDiscordEmbedBuilder(this Embeds embeds, YoutubeChannel channel)
    {
        try
        {
            Smart.Default.AddExtensions(new LimitFormatter());
            
            var color = ColorTranslator.FromHtml(embeds.Color);

            EmbedBuilder embedBuilder = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = embeds.Author is not null ? Smart.Format(embeds.Author, channel) : "",
                    Url = embeds.AuthorUrl is not null ? Smart.Format(embeds.AuthorUrl, channel) : "",
                    IconUrl = embeds.AuthorAvatarUrl is not null ? Smart.Format(embeds.AuthorAvatarUrl, channel) : ""
                },
                Title = embeds.Title is not null ? Smart.Format(embeds.Title, channel) : "",
                Url = embeds.TitleUrl is not null ? Smart.Format(embeds.TitleUrl, channel) : "",
                Description = embeds.Description is not null ? Smart.Format(embeds.Description, channel) : "",
                Color = new Discord.Color(color.R, color.G, color.B),
                ImageUrl = embeds.ImageUrl is not null ? Smart.Format(embeds.ImageUrl, channel) : "",
                ThumbnailUrl = embeds.ThumbnailUrl is not null ? Smart.Format(embeds.ThumbnailUrl, channel) : "",
                Footer = new EmbedFooterBuilder()
                {
                    Text = embeds.Footer is not null ? Smart.Format(embeds.Footer, channel) : "",
                    IconUrl = embeds.FooterUrl is not null ? Smart.Format(embeds.FooterUrl, channel) : ""
                }
            };

            if (embeds.Fields is not null)
            {
                foreach (EmbedFields fields in embeds.Fields)
                {
                    embedBuilder.AddField(Smart.Format(fields.Name, channel), Smart.Format(fields.Value, channel),
                        fields.Inline);
                }
            }

            Log.Debug($"Y2DL: Converted Embed to EmbedBuilder for YouTubeChannel {channel.Name}");

            return embedBuilder;
        }
        catch (Exception e)
        {
            Log.Warning(e, $"An error occured while converting");

            return default;
        }
    }
}