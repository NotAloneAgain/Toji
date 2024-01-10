using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Toji.BetterMap.API
{
    public static class Optimizer
    {
        private static HashSet<string> _names;
        private static HashSet<string> _tags;

        static Optimizer()
        {
            _names = [

                "Mice",
                "Obj",
                "SCP106_PORTAL",
                "Work Station",
                ";)"
            ];

            _tags = [];
        }

        public static void TryOptimize(this GameObject obj)
        {
            if (obj == null || !_names.Contains(obj.name) && !_tags.Contains(obj.tag) && !obj.TryGetComponent<WorkstationController>(out _))
            {
                return;
            }

            if (obj.TryGetComponent<WorkstationController>(out _) && Vector3.Distance(Vector3.zero, obj.transform.position) > 30)
            {
                return;
            }

            try
            {
                if (obj.TryGetComponent<NetworkIdentity>(out _))
                {
                    NetworkServer.Destroy(obj);
                }
                else
                {
                    GameObject.Destroy(obj);
                }
            }
            catch
            {
                obj.SetActive(false);
            }
        }
    }
}
