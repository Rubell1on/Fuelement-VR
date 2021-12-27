using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;

namespace CatchyClick {
    public class DataGridView : MonoBehaviour
    {
        [Header("Root components")]
        public GameObject columnsComponent;
        public GameObject rowsComponent;
        [Space(10)]

        [Header("Data lists")]
        [SerializeField]
        public List<DataGridViewColumn> columns;
        [SerializeField]
        public CustomList<DataGridViewRow> rows = new CustomList<DataGridViewRow>();
        [HideInInspector]
        public List<DataGridViewRowUI> uiRows = new List<DataGridViewRowUI>();
        [Space(10)]

        public Font tableFont;

        [Header("Row colors")]
        public bool colorRows = true;
        public Color even = Color.white;
        public Color odd = new Color32(174, 174, 174, 255);
        [Space(10)]

        [Header("Cells")]
        public Color cellTextColor = Color.black;
        [Space(10)]

        [Header("Events")]
        public DataGridViewCellClickEvent cellClicked = new DataGridViewCellClickEvent();
        public DataGridViewCellDoubleClickEvent cellDoubleClicked = new DataGridViewCellDoubleClickEvent();

        private List<bool> sortings = new List<bool>();

        private void Awake()
        {
            columns = _GetHeaderElements();

            if (sortings.Count > 0)
            {
                sortings.Clear();
            }

            columns.ForEach(h => sortings.Add(false));

            for (int i = 0; i < columns.Count; i++)
            {
                int id = i;
                DataGridViewColumn column = columns[id];

                if (column.sizeScaler != null)
                {
                    column.sizeScaler.scaleChanged.AddListener(size =>
                    {
                        uiRows.ForEach(r =>
                        {
                            RectTransform cellRT = r.cells[id].GetComponent<RectTransform>();
                            cellRT.sizeDelta = new Vector2(size.x, cellRT.sizeDelta.y);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(r.GetComponent<RectTransform>());
                        });
                    });
                }

                Button headerButton = column.GetComponent<Button>();
                
                headerButton.onClick.AddListener(() =>
                {
                    bool sorting = sortings[id];

                    rows.Sort((a, b) => {
                        string firstString = a.cells[id].value;
                        string secondString = b.cells[id].value;

                        int first;
                        int second;

                        if (Int32.TryParse(firstString, out first) && Int32.TryParse(secondString, out second))
                        {
                            return first.CompareTo(second);
                        }

                        return firstString.CompareTo(secondString);
                    });


                    if (sorting)
                    {
                        rows.Reverse();
                    }

                    sortings = sortings.Select(x => false).ToList();
                    sortings[id] = sorting ? false : true;

                    rows.changed?.Invoke();
                });
            }

            rows.changed.AddListener(OnChange);
        }

        private List<DataGridViewColumn> _GetHeaderElements()
        {
            DataGridViewColumn[] headers = columnsComponent.GetComponentsInChildren<DataGridViewColumn>();
            if (headers.Length > 0)
            {
                return headers.ToList();
            }

            return null;
        }

        public void OnChange()
        {
            if (rows.Count > 0)
            {
                if (rows?[0].cells.Count == columns?.Count)
                {
                    if (uiRows.Count > 0)
                    {
                        ClearRows();
                    }

                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataGridViewRow row = rows[i];
                        GameObject rowObject = DataGridViewRowUI.CreateRow();
                        DataGridViewRowUI rowUI = rowObject.GetComponent<DataGridViewRowUI>();

                        for (int j = 0; j < row.cells.Count; j++)
                        {

                            GameObject cell = DataGridViewCellUI.CreateCell();

                            Image cellImage = cell.GetComponent<Image>();
                            if (colorRows)
                            {
                                cellImage = cell.GetComponent<Image>();
                                cellImage.color = i == 0 || i % 2 == 0 ? even : odd;
                            }
                            else
                            {
                                cellImage.color = even;
                            }

                            RectTransform cellRT = cell.GetComponent<RectTransform>();
                            RectTransform headerRT = columns[j].GetComponent<RectTransform>();
                            cellRT.sizeDelta = new Vector2(headerRT.sizeDelta.x, cellRT.sizeDelta.y);

                            cell.transform.SetParent(rowUI.transform);
                            DataGridViewCellUI cellUI = cell.GetComponent<DataGridViewCellUI>();

                            if (tableFont)
                            {
                                cellUI.textComponent.font = tableFont;
                            }

                            cellUI.textComponent.color = cellTextColor;
                            cellUI.textComponent.text = row.cells[j].value;

                            Button cellButton = cell.GetComponent<Button>();
                            DataGridViewEventArgs args = new DataGridViewEventArgs(i, j);
                            cellButton.onClick.AddListener(OnClick);

                            rowUI.cells.Add(cellUI);

                            void OnClick()
                            {
                                cellClicked.Invoke(args);
                                cellDoubleClicked.Invoke(args);
                            }
                        }


                        uiRows.Add(rowUI);
                    }

                    uiRows.ForEach(r =>
                    {
                        r.transform.SetParent(rowsComponent.transform);
                        RectTransform rowRT = r.GetComponent<RectTransform>();
                        rowRT.anchoredPosition3D = Vector3.zero;
                        rowRT.localScale = Vector3.one;
                        rowRT.localRotation = Quaternion.Euler(Vector3.zero);
                    });

                }
                else
                {
                    throw new System.Exception("Cells count don't match with header!");
                }
            }
        }

        public void ClearRows()
        {
            uiRows.ForEach(r => Destroy(r.gameObject));
            uiRows.Clear();
        }
    }

    [Serializable]
    public class DataGridViewEventArgs
    {
        public int row;
        public int cell;

        public DataGridViewEventArgs(int row, int cell)
        {
            this.row = row;
            this.cell = cell;
        }
    }

    [Serializable]
    public class DataGridViewCellClickEvent : UnityEvent<DataGridViewEventArgs> { };
    [Serializable]
    public class DataGridViewCellDoubleClickEvent : DoubleClick<DataGridViewEventArgs> { };

    public static class StringExtention
    {
        public static int GetByteSumm(this string input)
        {
            return Encoding.UTF8.GetBytes(input).ToList().Aggregate(0, (acc, value) => acc + value);
        }
    }
}