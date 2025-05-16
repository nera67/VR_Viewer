using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class SceneData
{
    [Serializable]
    public class Marker
    {
        public float x;
        public float y;
        public string imagePath;

        public Marker(float x, float y, string imagePath)
        {
            this.x = x;
            this.y = y;
            this.imagePath = imagePath;
        }

        public Vector2 ToVector2() => new(x, y);
    }

    public List<Marker> markers = new();
    public string backgroundImagePath;

    public void SetMarkers(List<Marker> positions)
    {
        markers.Clear();
        foreach (var position in positions)
        {
            markers.Add(new Marker(position.x, position.y, position.imagePath));
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static SceneData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<SceneData>(json);
    }
}
