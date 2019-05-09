using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingSaveManger{
    private static readonly string FILE_NAME = "Settings";
    private static readonly string FILE_NAME_TITLES = "SettingsTitle";
    public static readonly string NULL_NAME = "NULL";
    
    /// <summary>
    /// セーブします
    /// </summary>
    /// <param name="name">セーブの名前</param>
    /// <param name="filePath">セーブしたいファイルパス文字列</param>
    /// <exception cref="ArgumentException">nullNameとnameが等しい時投げられます</exception>
    public static void save(string name,string filePath) {
        if(name == NULL_NAME)
            throw new ArgumentException("you tried to name NULL_NAME");
        Debug.Log("save");
        ES2.Save(filePath,FILE_NAME+".txt?tag=" + name);
    }

    public static void saveTitle(string name, string title) {
        if(name == NULL_NAME)
            throw new ArgumentException("you tried to name NULL_NAME");
        Debug.Log("save");
        ES2.Save(title,FILE_NAME_TITLES + ".txt?tag=" + name);
    }
    
    /// <summary>
    /// ロードします
    /// </summary>
    /// <param name="name">セーブの名前</param>
    /// <returns>ロードしたファイルパス</returns>
    /// <exception cref="ArgumentException">nullNameとnameが等しい時投げられます</exception>
    public static string load(string name) {
        if (name == NULL_NAME)
            throw new ArgumentException("NULL_NAME can't be a name");
        
        if (!ES2.Exists(FILE_NAME + ".txt?tag=" + name))
            return NULL_NAME;
        return ES2.Load<string>(FILE_NAME + ".txt?tag=" + name);
    }

    public static void delete(string name) {
        if (!ES2.Exists(FILE_NAME + ".txt?tag=" + name))
            return;
        ES2.Delete(FILE_NAME + ".txt?tag=" + name);
    }

    public static string loadTitle(string name) {
        if (name == NULL_NAME)
            throw new ArgumentException("NULL_NAME can't be a name");
        
        if (!ES2.Exists(FILE_NAME + ".txt?tag=" + name))
            return NULL_NAME;
        return ES2.Load<string>(FILE_NAME_TITLES + ".txt?tag=" + name);
    }
}
