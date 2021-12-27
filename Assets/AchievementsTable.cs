using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchyClick;

public class AchievementsTable : MonoBehaviour
{
    public DataGridView dataGridView;
    public AchievementsController achievementsController;

    private void OnEnable()
    {
        dataGridView.rows.AddRange(GenerateRows(achievementsController.achievements));
    }

    private void OnDisable()
    {
        dataGridView.ClearRows();
        dataGridView.rows.Clear();
    }

    public IEnumerable<DataGridViewRow> GenerateRows(IEnumerable<AchievementObject> achievements)
    {
        return achievements.Select(a =>
        {
            List<DataGridViewCell> cells = new List<DataGridViewCell>()
            {
                new DataGridViewCell(a.title),
                new DataGridViewCell(a.description),
                new DataGridViewCell(a.received ? a.receiveDate.ToString("dd.MM.yyyy") : "Не получено")
            };

            return new DataGridViewRow(cells);
        });
    }
}
