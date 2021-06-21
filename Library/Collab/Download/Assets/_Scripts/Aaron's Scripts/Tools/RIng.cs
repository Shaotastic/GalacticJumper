using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace ATools
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy("System.Collections.Generic.Mscorlib_CollectionDebugView<T>")]
    [DefaultMember("Item")]
    [System.Serializable]
    public class Ring<T>
    {
        //---------Variables---------
        private T[] list;

        private int index;

        private int head, tail, capacity, count;

        public int Capacity
        {
            get { return capacity; }
        }

        //---------Functions---------
        public Ring(int length)
        {
            capacity = length;
            list = new T[capacity];
            head = 0;
            tail = 0;
        }

        public void Push(T obj)
        {
            if (list[tail] == null)
            {
                list[tail] = obj;
                IncrementTail();
            }
            else
                return;
        }

        public T Pop()
        {
            if (list[head] == null)
            {
                return default(T);
            }

            T temp = list[head];

            list[head] = default(T);

            IncrementHead();

            return temp;
        }

        public T Peek()
        {
            return list[head];
        }

        void IncrementTail()
        {
            //Check next spot if its empty
            //If empty, move tail there
            // if not, dont do it
            tail++;

            if (tail == capacity)
            {
                tail = 0;

            }

            if (tail == head)
            {
                //  if(h)
            }

        }

        void IncrementHead()
        {
            head++;

            if (head == capacity)
                head = 0;
        }

        public T GetAt(int index)
        {
            return list[index];
        }

        public int GetTail()
        {
            return tail;
        }

        public int GetHead()
        {            
            return head;
        }

    }
}