//#define CLEAR

using UnityEngine;
using System.Collections.Generic;
using System;

public class Profile : MonoBehaviour
{
    public const string Separator = "|";


    public static SavedValues<StringSaveType, string> StringValues { get; private set; } = new SavedValues<StringSaveType, string>(
        new StringParser<StringSaveType>()
        {
            [StringSaveType.Nickname] = "Nickname"
        });

    public static SavedValues<IntegerSaveType, int> IntegerValues { get; private set; } = new SavedValues<IntegerSaveType, int>(
        new IntegerSaveParser<IntegerSaveType>()
        {
            [IntegerSaveType.Cash] = "Conis",
            [IntegerSaveType.UseBodySkin] = "CurrentBodySkin",
            [IntegerSaveType.UseBoardSkin] = "CurrentBoardSkin",
            [IntegerSaveType.CurrentLevel] = "CurrentLvl",
            [IntegerSaveType.LastCompletedLevel] = "LastCompletedLevel",
        });

    public static SavedList<IntegerListSaveType, int> IntegerListValues { get; private set; } = new SavedList<IntegerListSaveType, int>(
        new IntegerListKeys<IntegerListSaveType>()
        {
            [IntegerListSaveType.AvailableBodySkins] = "AvailableBodySkins",
            [IntegerListSaveType.AvailableBoardSkins] = "AvailableBoardSkins",
        });

    private void Awake()
    {
#if CLEAR
        PlayerPrefs.DeleteAll();
#endif

        IntegerListValues.AddItem(IntegerListSaveType.AvailableBodySkins, IntegerValues.GetValue(IntegerSaveType.UseBodySkin));
        IntegerListValues.AddItem(IntegerListSaveType.AvailableBoardSkins, IntegerValues.GetValue(IntegerSaveType.UseBoardSkin));
    }

    #region Objects of saved

    public class SavedList<TKey, TItem> : SavedValues<TKey, List<TItem>>
        where TItem : IComparable<TItem>
    {
        private ListSaveParser<TKey, TItem> _parser;


        public SavedList(ListSaveParser<TKey, TItem> parser)
            : base(parser)
        {
            _parser = parser;
        }

        public void AddItem(TKey key, TItem item)
        {
            _parser.AddItem(key, item);
        }

        public bool CheckItemExistence(TKey key, TItem value)
        {
            return _parser.CheckItemExistence(key, value);
        }
    }

    public class SavedValues<TKey, TValue>
    {
        private SaveParser<TKey, TValue> _keys;
        private EventStorages<TKey, TValue> _onValueUpdate;


        public SavedValues(SaveParser<TKey, TValue> keys)
        {
            _keys = keys;

            _onValueUpdate = new EventStorages<TKey, TValue>();

            _keys.OnSave += _onValueUpdate.Call;
        }

        public void Subscribe(TKey key, Action<TValue> subscriber)
        {
            _onValueUpdate.Subscribe(key, subscriber);
        }

        public void Unsubscribe(TKey key, Action<TValue> subscriber)
        {
            _onValueUpdate.Unsubscribe(key, subscriber);
        }

        public string GetKey(TKey key)
        {
            return _keys.GetKey(key);
        }

        public TValue GetValue(TKey key)
        {
            return _keys.GetValue(key);
        }

        public void Save(TKey key, TValue value)
        {
            _keys.Save(key, value);
        }
    }

    #endregion

    #region Parsers of list

    public class IntegerListKeys<TKey> : ListSaveParser<TKey, int>
    {
        protected override Func<string, int> ParseItem => int.Parse;

        protected override Func<int, string> ItemToString => value => value.ToString();
    }


    public abstract class ListSaveParser<TKey, TItem> : SaveParser<TKey, List<TItem>>
        where TItem : IComparable<TItem>
    {
        public string Separator => Profile.Separator;

        protected override List<TItem> DefaultValue => new List<TItem>();

        protected sealed override Func<string, List<TItem>> GetSavedValue => OverrideGetSavedValue;

        protected sealed override Action<string, List<TItem>> SaveValue => OverrideSaveValue;

        protected abstract Func<string, TItem> ParseItem { get; }

        protected abstract Func<TItem, string> ItemToString { get; }

        public void AddItem(TKey key, TItem item)
        {
            if (CheckItemExistence(key, item) == true)
            {
                return;
            }

            var value = GetValue(key);
            value.Add(item);

            Save(key, value);
        }

        public bool CheckItemExistence(TKey key, TItem item)
        {
            foreach (var checkItem in GetValue(key))
            {
                if (checkItem.CompareTo(item) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private List<TItem> OverrideGetSavedValue(string key)
        {
            var list = new List<TItem>();
            string[] values = PlayerPrefs.GetString(key).Split(Separator);

            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    list.Add(ParseItem(value));
                }
            }

            return list;
        }

        private void OverrideSaveValue(string key, List<TItem> value)
        {
            string stringValue = "";

            foreach (var item in value)
            {
                stringValue += Separator + ItemToString(item);
            }

            PlayerPrefs.SetString(key, stringValue);
        }
    }

    #endregion

    #region Parsers of saves

    public class StringParser<TKey> : SaveParser<TKey, string>
    {
        protected override string DefaultValue => "";

        protected override Func<string, string> GetSavedValue => PlayerPrefs.GetString;

        protected override Action<string, string> SaveValue => PlayerPrefs.SetString;
    }


    public class IntegerSaveParser<TKey> : SaveParser<TKey, int>
    {
        protected override int DefaultValue => 0;

        protected override Func<string, int> GetSavedValue => PlayerPrefs.GetInt;

        protected override Action<string, int> SaveValue => PlayerPrefs.SetInt;
    }

    public abstract class SaveParser<TKey, TValue>
    {
        public event Action<TKey, TValue> OnSave;

        private Dictionary<TKey, string> _keys = new Dictionary<TKey, string>();


        public string this[TKey key] { get => GetKey(key); set => SetKey(key, value); }

        protected abstract TValue DefaultValue { get; }

        protected abstract Func<string, TValue> GetSavedValue { get; }

        protected abstract Action<string, TValue> SaveValue { get; }

        public string GetKey(TKey key)
        {
            if (_keys.ContainsKey(key) == true)
            {
                return _keys[key];
            }

            return "";
        }

        public void SetKey(TKey key, string value)
        {
            _keys[key] = value;
        }

        public TValue GetValue(TKey key)
        {
            if (CheckKeySave(key) == true)
            {
                return GetSavedValue(_keys[key]);
            }

            return DefaultValue;
        }

        public void Save(TKey key, TValue value)
        {
            if (_keys.ContainsKey(key) == false)
            {
                return;
            }

            SaveValue(_keys[key], value);
            OnSave?.Invoke(key, value);
        }

        private bool CheckKeySave(TKey key)
        {
            return _keys.ContainsKey(key) == true && PlayerPrefs.HasKey(_keys[key]) == true;
        }
    }

    #endregion
}