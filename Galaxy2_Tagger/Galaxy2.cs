using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Galaxy2_Tagger
{
    public class Galaxy2:IDisposable
    {
        public SqliteConnection db=null;
        public JavaScriptSerializer ser = new JavaScriptSerializer();
        public Galaxy2()
        {
            db = new SqliteConnection("Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\GOG.com\\Galaxy\\storage\\galaxy-2.0.db;");
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.CommandText = "PRAGMA journal_mode=WAL";
                command.ExecuteNonQuery();
            }
        }
        private long _userId = -1;
        public long UserId
        {
            get
            {
                if (_userId < 0)
                {
                    using(var cmd = db.CreateCommand()){
                        cmd.CommandText = "SELECT Id FROM Users LIMIT 1";
                        _userId = (long)cmd.ExecuteScalar();
                    }
                }
                return _userId;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Close();
                db.Dispose();
                db = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public List<Game> ReadGames()
        {
            var lines = new Dictionary<string,Game>();

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText= "SELECT p.releaseKey,p.title,t.tag FROM ( select releaseKey,max(\"value\") AS Title from GamePieces where gamePieceTypeId in (4726,1347) group by releaseKey ) AS p LEFT JOIN UserReleaseTags AS t on p.releaseKey=t.releaseKey WHERE p.releaseKey IN (SELECT releaseKey FROM ReleaseProperties WHERE isDlc=0 AND isVisibleInLibrary=1) ORDER by p.title ASC";
                var r = cmd.ExecuteReader();
                while (r.Read())
                {

                    var tag = "";

                    if(!r.IsDBNull(2))
                        tag=r.GetString(2);

                    var id = r.GetString(0);
                    if (!lines.ContainsKey(id))
                        lines.Add(id, new Game() { id = id, title = ser.Deserialize<Dictionary<string, string>>(r.GetString(1))["title"], tags = new string[0], instance = this });

                    if (tag != "")
                        lines[id].tags = new List<string>(lines[id].tags) { tag }.ToArray();
                    
                }
            }

            return new List<Game>(lines.Values);
        }

    }
}
