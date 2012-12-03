using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Entities
{
    public class Movement
    {
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint MM_GetTime();

        protected uint m_lastMoveTime;
        protected uint m_totalMovingTime;
        protected uint m_desiredEndMovingTime;
        protected Position m_destination;
        protected internal Monster m_owner;
        public bool m_moving, m_MayMove;
        public float movementSpeed = 4.0f;
        public byte MoveState = 0;

        public Movement(Monster owner)
        {
            m_owner = owner;
            m_MayMove = true;
        }

        public virtual uint RemainingTime
        {
            get
            {
                float speed, distanceToTarget;

                speed = m_owner.MovementSpeed;
                distanceToTarget = m_owner.GetDistance(ref m_destination);

                return (uint)(distanceToTarget / speed) * 1000;
            }
        }

        public bool MoveTo(Position destination)
        {
            // TODO: Figure out when to send a query
            MoveState = 0;
            return MoveTo(destination, false);
        }

        public bool MoveTo(Position destination, bool findPath)
        {
            m_destination = destination;

            // start moving
            MoveToDestination();

            // cannot move
            return false;
        }

        public byte Update()
        {
            byte t = 0;

            if (!m_moving)
            {
                // is not moving
                return t;
            }

            if (UpdatePosition(out t))
            {
                // arrived
                return t;
            }

            // still going
            return t;
        }

        public void Stop()
        {
            if (m_moving)
            {
                byte t;
                UpdatePosition(out t);

                m_moving = false;
            }
        }

        protected void MoveToDestination()
        {
            m_moving = true;

            m_totalMovingTime = RemainingTime;

            m_lastMoveTime = MM_GetTime(); //(uint)Environment.TickCount;
            m_desiredEndMovingTime = m_lastMoveTime + m_totalMovingTime;
        }

        protected bool UpdatePosition(out byte result)
        {
            byte ret = 0;
            m_totalMovingTime = RemainingTime;
            var currentTime = MM_GetTime(); //(uint)Environment.TickCount;

            // ratio between time passed since last update and total movement time
            var delta = (currentTime - m_lastMoveTime) / (float)m_totalMovingTime;

            // if the ratio is more than 1, then we have reached the target
            float distance = m_owner.Position.GetDistance(m_destination);
            if (distance <= m_owner.MovementSpeed + 1)
            {
                // move target directly to the destination
                m_owner.Position = m_destination;
                m_moving = false;
                MoveState = 2;
                result = MoveState;
                return true;
            }

            // otherwise we've passed delta part ofthe path
            var currentPos = m_owner.Position;
            var newPosition = currentPos + (m_destination - currentPos) * delta;

            switch (MoveState)
            {
                case 0:
                    MoveState = 1;
                    break;
                case 1:
                    ret = MoveState;
                    break;
                case 2:
                    ret = MoveState;
                    break;
            }

            m_lastMoveTime = currentTime;
            m_owner.Position = newPosition;

            m_owner.Position = new Position((float)Math.Round(m_owner.Position.X), (float)Math.Round(m_owner.Position.Y), (float)Math.Round(m_owner.Position.Z));
            result = ret;
            return false;
        }
    }
}