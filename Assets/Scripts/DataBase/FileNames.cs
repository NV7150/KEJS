using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileNames {
    private static Dictionary<string, String> names = new Dictionary<string, string>();

    public static String getName(String title) {
        if (names.ContainsKey(title)) {
            return names[title];
        }
        return "NONE";
    }

    public static void setName(String title,string name) {
        if (!names.ContainsKey(title)) {
            names.Add(title,name);
        } else {
            names[title] = name;
        }
    }

    public static void deleteName(string title) {
        if (names.ContainsKey(title))
            names.Remove(title);
    }
    
    public static String urlToFileName(string rawUrl) {
        var strings = rawUrl.Split('/', '¥');
        return strings[strings.Length - 1];
    }
}
