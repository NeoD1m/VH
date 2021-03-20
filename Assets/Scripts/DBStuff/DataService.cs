using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName){

#if UNITY_EDITOR
            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log("Final PATH: " + dbPath);     

	}

    /*public void CreateDB(){
		_connection.DropTable<Person> ();
		_connection.CreateTable<Person> ();

		_connection.InsertAll (new[]{
			new Person{
				Id = 1,
				Name = "Tom",
				Surname = "Perez",
				Age = 56
			},
			new Person{
				Id = 2,
				Name = "Fred",
				Surname = "Arthurson",
				Age = 16
			},
			new Person{
				Id = 3,
				Name = "John",
				Surname = "Doe",
				Age = 25
			},
			new Person{
				Id = 4,
				Name = "Roberto",
				Surname = "Huertas",
				Age = 37
			}
		});
	} */
    public Dialog2 GetDialogById(int d_id)
    { //Получить диалог по ID
        Dialog2 res = new Dialog2();
        foreach (var tmp in _connection.Table<Dialog2>().Where(x => x.id == d_id))
            res = tmp;
        return res;
    }
    /*
        public IEnumerable<dialogs> GetPersons(){ //Стоковая функция. Мы использовали её только чтобы понять, что скрипты работают
            return _connection.Table<dialogs>();
        }

        public IEnumerable<dialogs> GetDialog(int dialog_id)    // Эта функция возвращает диалог по его id в БД
        {
            return _connection.Table<dialogs>().Where(x=> x.id == dialog_id );
        }

        public quest_templates GetQuestTemplate(int quest_id)    // Эта функция возвращает шаблон квеста по его id в БД
        {
            quest_templates res = new quest_templates();
            foreach (var a in _connection.Table<quest_templates>().Where(x => x.id == quest_id))
                res = a;
            return res;
        }

        public IEnumerable<quest_active> GetQuestsActive()    // Эта функция возвращает активные квесты
        {
            return _connection.Table<quest_active>();
        }
        public quest_active GetQuestActiveByType(int type)    // Эта функция возвращает активный квест по его типу в БД
        {
            quest_active res = new quest_active();
            foreach (var a in _connection.Table<quest_active>().Where(x => x.quest == type))
                res = a;
            return res;
        }

        public IEnumerable<quest_active> GetQuestsActiveByType(int type)    // Эта функция возвращает активные квесты по их типу в БД
        {
            return _connection.Table<quest_active>().Where(x => x.quest == type);
        }
        public void DeleteFromQuests(int id)                 // Удалить квест
        {
            _connection.MyDeleteById("quest_active", id);
        }
        public IEnumerable<quest_completed> GetCompletedQuests()    // Эта функция возвращает выполненные квесты
        {
            return _connection.Table<quest_completed>();
        }
        public void ClearCompletedQuests()          //Очистить БД с выполненными квестами
        {
            var a = GetCompletedQuests();
            foreach (var b in a)
                _connection.MyDeleteById("quest_completed", b.id);
        }

        public void ClearActiveQuests()
        {
            var a = GetQuestsActive();
            foreach (var b in a)
                _connection.MyDeleteById("quest_active", b.id);
        }

        public void AddQuestInActive(int quest_id, int DoE, int value)             // Добавить квест в активные
        {
            var i = new quest_active {quest = quest_id, DoE = DoE, value = value };
            Debug.Log(i.ToString());
            _connection.Insert(i);
        }

        public void AddQuestInCompleted(int quest_id)             // Добавить квест в выполненные
        {
            var i = new quest_completed { quest = quest_id};
            _connection.Insert(i);
        }

        public IEnumerable<item> GetItem(int id)    // Эта функция возвращает item по его id в БД
        {
            return _connection.Table<item>().Where(x => x.id == id);
        }

        public IEnumerable<inventory> GetInventoryItems()    // Эта функция возвращает все элементы инвентаря
        {
            return _connection.Table<inventory>();
        }

        public void AddItemInInventory(int item_id)             // Добавить предмет в инвентарь
        {
            var i = new inventory { item_id = item_id };
            _connection.Insert(i);
        }

        public void DeleteFromInventory(int id)                 // Очистить ячейку инвентаря
        {
            //_connection.Delete(i);
            _connection.MyDeleteById("inventory", id);
        }
        public void DeleteFromInventoryByItemId(int item_id)    // Удалить из инвентаря все предметы определённого типа
        {
            _connection.MyDeleteFromInventoryByCategory(item_id);
        }
        public void ClearInventory()                            // Очистить инвентарь
        {
            _connection.ClearTable("inventory");
        }
    */

    /*

        public Person CreatePerson(){          //НЕ УДАЛЯТЬ!!!!!!!!!! Эта функция может помочь в будущем 
            var p = new Person{
                    Name = "Johnny",
                    Surname = "Mnemonic",
                    Age = 21
            };
            _connection.Insert (p);
            return p;
        } */
}

