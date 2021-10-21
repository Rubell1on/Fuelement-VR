using System.Collections;
using UnityEngine;

public interface ISimpleDataCard
{
    void setCardData(ASimpleData data);
    void setBackground(Texture texture);
    void setTitle(string text);
    void setDescription(string text);
}