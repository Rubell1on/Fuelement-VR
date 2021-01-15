using UnityEngine;
using UnityEngine.UI;

public class MusicPlayerWindow : MonoBehaviour
{
    public Text songName;
    public Text artists;
    public Text albumName;
    public Text year;
    public RawImage albumIcon;

    void Start()
    {
        RectTransform albumTransform = albumIcon.GetComponent<RectTransform>();
        Vector2 startSize = albumTransform.sizeDelta;
        Vector2 targetSize = startSize + new Vector2(5, 5);

        LTSeq seq =  LeanTween.sequence();
        seq
            .append(LeanTween.size(albumTransform, targetSize, 2))
            .append(LeanTween.size(albumTransform, startSize, 2))
            .append(LeanTween.size(albumTransform, targetSize, 2))
            .append(LeanTween.size(albumTransform, startSize, 2));
    }
    public void SetData(MusicTrack track)
    {
        songName.text = track.songName.Length > 0 ? track.songName : "";
        albumName.text = track.albumName.Length > 0 ?  track.albumName : "";
        string artistsString = string.Join(", ", track.artists);
        artists.text = artistsString.Length > 0 ? artistsString : "";
        year.text = track.year != 0 ? track.year.ToString() : "";
        albumIcon.texture = track.albumIcon != null ? track.albumIcon : null;
    }
}
