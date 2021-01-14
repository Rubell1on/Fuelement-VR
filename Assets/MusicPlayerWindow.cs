using UnityEngine;
using UnityEngine.UI;

public class MusicPlayerWindow : MonoBehaviour
{
    public Text songName;
    public Text artists;
    public Text albumName;

    public void SetData(MusicTrack track)
    {
        songName.text = track.songName;
        albumName.text = track.albumName;
        string artistsString = string.Join(", ", track.artists);
        artists.text = artistsString;
    }
}
