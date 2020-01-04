using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy2_Tagger
{
    public class Game
    {
        public Galaxy2 instance = null;
        public string id;
        public string title;
        public string[] tags;
        public override string ToString()
        {
            return title;
        }

        public void AddTag(string tag)
        {
            if (tags.Contains(tag))
                return;

            var list = new List<string>(tags){tag};

            SetGamePiecesTags(list);
            InsertUserReleaseTag(tag);

            tags = list.ToArray();
        }
        public void RemoveTag(string tag)
        {
            if (!tags.Contains(tag))
                return;

            var list = new List<string>(tags);
            list.Remove(tag);

            SetGamePiecesTags(list);
            DeleteUserReleaseTag(tag);

            tags = list.ToArray();
        }


        private void SetGamePiecesTags(List<string> tags)
        {
            long gamePieces = 0;
            using (var cmd = instance.db.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM GamePieces WHERE ReleaseKey=@releaseKey AND userId=@id AND gamePieceTypeId=3";
                cmd.Parameters.Add("@releaseKey", SqliteType.Text);
                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters["@releaseKey"].Value = id;
                cmd.Parameters["@id"].Value = instance.UserId;
                gamePieces = (long)cmd.ExecuteScalar();
            }

            using (var cmd = instance.db.CreateCommand())
            {
                if(gamePieces==0)
                    cmd.CommandText = "INSERT INTO GamePieces(ReleaseKey,userId,value,gamePieceTypeId) VALUES(@releaseKey,@id,@tags,3)";
                else
                    cmd.CommandText = "UPDATE GamePieces SET value=@tags WHERE ReleaseKey=@releaseKey AND userId=@id AND gamePieceTypeId=3";
                cmd.Parameters.Add("@releaseKey", SqliteType.Text);
                cmd.Parameters.Add("@tags", SqliteType.Text);
                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters["@releaseKey"].Value = id;
                cmd.Parameters["@id"].Value = instance.UserId;
                cmd.Parameters["@tags"].Value = instance.ser.Serialize(new Dictionary<string, List<string>>() { { "tags", tags } });
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteUserReleaseTag(string tag)
        {
            using (var cmd = instance.db.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM UserReleaseTags WHERE userId=@id AND releaseKey=@releaseKey AND tag=@tag";
                cmd.Parameters.Add("@releaseKey", SqliteType.Text);
                cmd.Parameters.Add("@tag", SqliteType.Text);
                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters["@releaseKey"].Value = id;
                cmd.Parameters["@id"].Value = instance.UserId;
                cmd.Parameters["@tag"].Value = tag;
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertUserReleaseTag(string tag)
        {
            long userReleaseTags = 0;
            using (var cmd = instance.db.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM UserReleaseTags WHERE userId=@id AND releaseKey=@releaseKey AND tag=@tag";
                cmd.Parameters.Add("@releaseKey", SqliteType.Text);
                cmd.Parameters.Add("@tag", SqliteType.Text);
                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters["@releaseKey"].Value = id;
                cmd.Parameters["@id"].Value = instance.UserId;
                cmd.Parameters["@tag"].Value = tag;
                userReleaseTags = (long)cmd.ExecuteScalar();
            }

            if (userReleaseTags == 0) using (var cmd = instance.db.CreateCommand()){
                cmd.CommandText = "INSERT INTO UserReleaseTags(userId,releaseKey,tag) VALUES(@id,@releaseKey,@tag)";
                cmd.Parameters.Add("@releaseKey", SqliteType.Text);
                cmd.Parameters.Add("@tag", SqliteType.Text);
                cmd.Parameters.Add("@id", SqliteType.Integer);
                cmd.Parameters["@releaseKey"].Value = id;
                cmd.Parameters["@id"].Value = instance.UserId;
                cmd.Parameters["@tag"].Value = tag;
                    try
                    {
                        //FIXME: sometimes this runs into a unique constraint... have to investigate
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { }
            }
        }
    }
}
