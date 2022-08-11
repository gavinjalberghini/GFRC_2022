using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static Global;

public class Leaderboard : MonoBehaviour
{
	public Transform       entries_container;
	public GameObject      entry_prefab;
	public ClickDetector[] columns_header;
	public GameObject   [] columns_sort_arrow;
	public Button          btn_main_menu;

	int  last_clicked_column_index = -1;
	bool decending;

	void Start()
	{
		for (int i_ = 0; i_ < columns_header.Length; i_ += 1)
		{
			int i = i_;
			columns_header[i].click = delegate {
				if (last_clicked_column_index == i)
				{
					decending = !decending;
				}
				else
				{
					decending = false;
				}

				for (int j = 0; j < columns_sort_arrow.Length; j += 1)
				{
					columns_sort_arrow[j].SetActive(j == i);
				}
				set_local_rotation_z(columns_sort_arrow[i].transform, decending ? 180.0f : 0.0f);

				var entries = db_get_entries();
				{
					entries.Sort((DB_Entry a, DB_Entry b) =>
						a.points == b.points ? a.unixtime.CompareTo(b.unixtime) :
						a.points ==       -1 ?  1 :
						b.points ==       -1 ? -1 : -a.points.CompareTo(b.points)
					);
					for (int i = 0; i < entries.Count && entries[i].points != -1; i += 1)
					{
						entries[i].username = (i + 1) + ". " + entries[i].username;
					}
				}

				switch (i)
				{
					case 0:
					{
						entries.Sort((DB_Entry a, DB_Entry b) => a.username.CompareTo(b.username));
					} break;

					case 1:
					{
						entries.Sort((DB_Entry a, DB_Entry b) =>
							a.teamnumber == b.teamnumber ? 0 :
							a.teamnumber ==           "" ? (decending ? -1 :  1) :
							b.teamnumber ==           "" ? (decending ?  1 : -1) : int.Parse(a.teamnumber).CompareTo(int.Parse(b.teamnumber))
						);
					} break;

					case 2:
					{
						entries.Sort((DB_Entry a, DB_Entry b) =>
							a.points == b.points ? 0 :
							a.points ==       -1 ? (decending ? -1 :  1) :
							b.points ==       -1 ? (decending ?  1 : -1) : -a.points.CompareTo(b.points)
						);
					} break;

					case 3:
					{
						entries.Sort((DB_Entry a, DB_Entry b) =>
							a.build == b.build ? 0 :
							a.build ==      "" ? (decending ? -1 :  1) :
							b.build ==      "" ? (decending ?  1 : -1) : a.build.CompareTo(b.build)
						);
					} break;

					case 4:
					{
						entries.Sort((DB_Entry a, DB_Entry b) =>
							a.unixtime == b.unixtime ? 0 :
							a.unixtime ==          0 ? (decending ? -1 :  1) :
							b.unixtime ==          0 ? (decending ?  1 : -1) : -a.unixtime.CompareTo(b.unixtime)
						);
					} break;
				}

				rebuild(entries);
				last_clicked_column_index = i;
			};
		}

		btn_main_menu.onClick.AddListener(delegate {
			SceneManager.LoadScene("Scenes/Title Menu Scene");
		});

		columns_header[2].click(null);
	}

	void rebuild(List<DB_Entry> entries)
	{
		foreach (Transform x in entries_container)
		{
			Destroy(x.gameObject);
		}

		if (decending)
		{
			entries.Reverse();
		}

		foreach (var x in entries)
		{
			GameObject entry = Instantiate(entry_prefab, entries_container);
			entry.transform.Find("Username"   ).GetComponent<TMP_Text>().text = x.username;
			entry.transform.Find("Team Number").GetComponent<TMP_Text>().text = x.teamnumber == "" ? "N/A" : x.teamnumber;
			entry.transform.Find("High Score" ).GetComponent<TMP_Text>().text = x.points == -1 ? "N/A" : x.points.ToString();
			entry.transform.Find("Build"      ).GetComponent<TMP_Text>().text = x.build == "" ? "N/A" : x.build;
			entry.transform.Find("Date"       ).GetComponent<TMP_Text>().text = x.unixtime == 0 ? "N/A" : (new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).AddSeconds(x.unixtime).ToLocalTime().ToString();

			if (db_currently_signed_in && db_curr_username == x.username)
			{
				entry.GetComponent<Image>().enabled = true;
			}
			else
			{
				entry.GetComponent<Image>().enabled = false;
			}
		}
	}
}
