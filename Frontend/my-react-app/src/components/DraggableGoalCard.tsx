import React from "react";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { Goal } from "../services/apiService";

interface DraggableGoalCardProps {
  goal: Goal;
  onEdit: (goal: Goal) => void;
}

export const DraggableGoalCard: React.FC<DraggableGoalCardProps> = ({
  goal,
  onEdit,
}) => {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({
    id: goal.id,
    data: {
      type: "goal",
      goal,
      status: goal.status,
    },
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
    cursor: "grab",
    touchAction: "none",
  };

  const getPriorityColor = (priority: string) => {
    const priorityStr = String(priority).toLowerCase();
    switch (priorityStr) {
      case "high":
        return "bg-red-100 text-red-800";
      case "medium":
        return "bg-yellow-100 text-yellow-800";
      case "low":
        return "bg-green-100 text-green-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const getStatusColor = (status: string) => {
    const statusStr = String(status).toLowerCase();
    switch (statusStr) {
      case "completed":
        return "bg-green-100 text-green-800";
      case "inprogress":
        return "bg-blue-100 text-blue-800";
      case "notstarted":
        return "bg-gray-100 text-gray-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300"
      onDoubleClick={(e) => {
        e.preventDefault();
        onEdit(goal);
      }}
    >
      <div className="p-6">
        <div className="flex justify-between items-start mb-4">
          <h3 className="text-xl font-semibold text-gray-800">{goal.title}</h3>
          <div className="flex gap-2">
            <span
              className={`px-2 py-1 rounded-full text-xs font-semibold ${getPriorityColor(
                goal.priority
              )}`}
            >
              {goal.priority}
            </span>
            <span
              className={`px-2 py-1 rounded-full text-xs font-semibold ${getStatusColor(
                goal.status
              )}`}
            >
              {goal.status}
            </span>
          </div>
        </div>
        <p className="text-gray-600 mb-4">{goal.description}</p>
        {goal.dueDate && (
          <div className="text-sm text-gray-500">
            Deadline: {new Date(goal.dueDate).toLocaleDateString()}
          </div>
        )}
        {goal.isPublic && (
          <div className="mt-2">
            <span className="bg-blue-100 text-blue-800 text-xs font-semibold px-2 py-1 rounded-full">
              Public
            </span>
          </div>
        )}
        {goal.tags && goal.tags.length > 0 && (
          <div className="mt-2 flex flex-wrap gap-1">
            {goal.tags.map((tag, index) => (
              <span
                key={index}
                className="bg-gray-100 text-gray-800 text-xs font-semibold px-2 py-1 rounded-full"
              >
                {typeof tag === "string" ? tag : tag.name}
              </span>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};
