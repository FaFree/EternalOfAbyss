using System;
using ECS.Scripts.Components;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scripts;
using UnityEngine;

namespace ECS.Scripts.Providers
{
    public class ArrowProvider : MonoProvider<ArrowMarker>
    {
        private Event<OnArrowCollisionEnter> onArrowCollisionEnter;
        private void Start()
        {
            this.onArrowCollisionEnter = World.Default.GetEvent<OnArrowCollisionEnter>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Walls"))
            {
                this.onArrowCollisionEnter.NextFrame(new OnArrowCollisionEnter
                {
                    collision = collision,
                    entityId = this.Entity.ID,
                });
                
                var arrowEntity = this.Entity;

                if (arrowEntity == null)
                    return;

                var boostModel = WorldModels.Default.Get<BoostsModel>();
                
                ref var arrowComponent = ref arrowEntity.GetComponent<ArrowComponent>();
                
                ref var arrowTransform = ref arrowEntity.GetComponent<TransformComponent>().transform;

                if (arrowComponent.collisionCount > 0 && boostModel.isReboundArrow)
                {
                    Ray ray = new Ray(arrowTransform.position, arrowComponent.direction);
                    RaycastHit hit;
                    
                    if (Physics.Raycast(ray, out hit))
                    {
                        var incomingDirection = arrowComponent.direction;
                        var normal = hit.normal;

                        var newDirection = Vector3.Reflect(incomingDirection, normal);

                        arrowComponent.direction = new Vector3(newDirection.x, 0, newDirection.z).normalized;

                        Quaternion rotation = Quaternion.LookRotation(arrowComponent.direction);

                        arrowTransform.rotation = rotation;
                        arrowComponent.collisionCount--;
                    }
                }
                else
                {
                    Destroy(arrowTransform.gameObject);
                    World.Default.RemoveEntity(arrowEntity);
                }
            }
        }
    }
}