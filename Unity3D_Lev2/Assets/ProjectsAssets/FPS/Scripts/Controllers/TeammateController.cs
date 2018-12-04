using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS
{
    public class TeammateController : MonoBehaviour
    {
        //событие для вью
        public static UnityAction<TeammateModel> OnTeammateSelected;
        //ссылка на выбранный тиммэйт
        private TeammateModel _currentTeammate;
        private Dictionary<TeammateModel, Queue<Vector3>> teamsPath; 

        public void MoveCommand() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                TeammateModel teammate = hit.collider.GetComponent<TeammateModel>();
                if (teammate)
                    SelectTeammate(teammate);
                else if (_currentTeammate) _currentTeammate.SetDestination(hit.point);
            }
        }

        public void FollowPlayerCommand()
        {
            if (_currentTeammate) _currentTeammate.SwitchFollow();
        }

        public void SetPathCommand()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                TeammateModel teammate = hit.collider.GetComponent<TeammateModel>();
                if (teammate)
                {
                    SelectTeammate(teammate);

                    //foreach (var obj in teamsPath)
                    //{
                    //    var queue = new Queue<Vector3>();
                    //    for (int i = 0; i < poolObj.ObjectsCount; i++)
                    //    {
                    //        GameObject go = Instantiate(obj);
                    //        go.SetActive(false);
                    //        queue.Enqueue(go.GetComponent<Vector3>());
                    //    }
                    //    teamsPath.Add(poolObj.PoolID, queue);
                    //}
                }
                else if (_currentTeammate) _currentTeammate.SetDestination(hit.point);
            }
        }

        public void SelectTeammate(TeammateModel teammateModel)
        {
            _currentTeammate = teammateModel;
            if (OnTeammateSelected != null)
                OnTeammateSelected(teammateModel);
        }
    }
}