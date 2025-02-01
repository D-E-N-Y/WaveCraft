using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcessorSystem : GameSystem
{
    public static ProcessorSystem current;
    
    Dictionary<E_Resource, List<IProcessor>> processors;

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        processors = new Dictionary<E_Resource, List<IProcessor>>();
    }

    public void AddProcessor(E_Resource resource, IProcessor processor)
    {
        if(!processors.ContainsKey(resource))
        {
            processors[resource] = new List<IProcessor>();
        }

        processors[resource].Add(processor);
    }

    public void RemoveProcessor(E_Resource resource, IProcessor processor)
    {
        processors[resource].Remove(processor);
    }

    public IProcessor GetNearbyProcessor(Vector3 target, E_Resource resource)
    {
        Debug.Log(processors[resource]);
        return processors[resource].OrderBy(processor => Vector3.Distance(processor.GetPosition(), target)).First();
    }
}