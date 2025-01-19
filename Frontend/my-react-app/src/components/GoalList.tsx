import React, { useState, useEffect } from "react";
import apiService, { Goal } from "../services/apiService";
import EditGoalModal from "./EditGoalModal";
import { DndContext, DragEndEvent, closestCenter } from "@dnd-kit/core";
import { FaPlus } from "react-icons/fa";
import {
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { DraggableGoalCard } from "./DraggableGoalCard";
import { useDroppable } from "@dnd-kit/core";

type GoalStatus = "NotStarted" | "InProgress" | "Completed";

const DroppableContainer = ({
  status,
  children,
}: {
  status: GoalStatus;
  children: React.ReactNode;
}) => {
  const { setNodeRef } = useDroppable({
    id: `droppable-${status}`,
    data: {
      type: "container",
      status: status,
    },
  });

  return (
    <div
      ref={setNodeRef}
      className="bg-gray-50 p-4 rounded-lg min-h-[200px] h-full"
    >
      {children}
    </div>
  );
};

const GoalList: React.FC = () => {
  const [goals, setGoals] = useState<Goal[]>([]);
  const [selectedGoal, setSelectedGoal] = useState<Goal | null>(null);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);

  const fetchGoals = async () => {
    try {
      const fetchedGoals = await apiService.getUserGoals();
      console.log("Fetched goals:", fetchedGoals);
      setGoals(fetchedGoals);
    } catch (error) {
      console.error("Error fetching goals:", error);
    }
  };

  useEffect(() => {
    fetchGoals();
  }, []);

  const handleEditClick = (goal: Goal) => {
    const goalWithCompleted = {
      ...goal,
      isCompleted: goal.status === "Completed",
    };
    setSelectedGoal(goalWithCompleted);
    setShowEditModal(true);
  };

  const handleModalClose = () => {
    setShowEditModal(false);
    setShowCreateModal(false);
    setSelectedGoal(null);
    fetchGoals();
  };

  const handleCreateClick = () => {
    setSelectedGoal(null);
    setShowCreateModal(true);
  };

  const handleDragEnd = async (event: DragEndEvent) => {
    const { active, over } = event;
    if (!over) return;

    const goalId = active.id as string;
    const newStatus = (over.data.current as { status: GoalStatus })?.status;
    const goalToUpdate = goals.find((g) => g.id === goalId);

    console.log("Drag end details:", {
      goalId,
      newStatus,
      currentGoal: goalToUpdate,
      activeData: active.data,
      overData: over.data,
      activeId: active.id,
      overId: over.id,
    });

    if (goalToUpdate && newStatus && goalToUpdate.status !== newStatus) {
      try {
        const updatedGoal = {
          ...goalToUpdate,
          status: newStatus,
        };
        console.log("Updating goal with:", updatedGoal);
        await apiService.updateGoal(goalId, updatedGoal);
        fetchGoals();
      } catch (error) {
        console.error("Error updating goal status:", error);
      }
    }
  };

  const statusSections: GoalStatus[] = [
    "NotStarted",
    "InProgress",
    "Completed",
  ];
  const getStatusTitle = (status: GoalStatus) => {
    switch (status) {
      case "NotStarted":
        return "Not Started";
      case "InProgress":
        return "In Progress";
      case "Completed":
        return "Completed";
      default:
        return status;
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Your Goals</h2>
        <button
          onClick={handleCreateClick}
          className="flex items-center gap-2 bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg transition-colors"
        >
          <FaPlus /> Create Goal
        </button>
      </div>

      <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 h-full">
          {statusSections.map((status) => (
            <div key={status} className="flex flex-col h-full">
              <h3 className="text-lg font-semibold mb-4">
                {getStatusTitle(status)}
              </h3>
              <DroppableContainer status={status}>
                <SortableContext
                  items={goals
                    .filter((g) => g.status === status)
                    .map((g) => g.id)}
                  strategy={verticalListSortingStrategy}
                >
                  <div className="space-y-4">
                    {goals
                      .filter((goal) => goal.status === status)
                      .map((goal) => (
                        <DraggableGoalCard
                          key={goal.id}
                          goal={goal}
                          onEdit={handleEditClick}
                        />
                      ))}
                  </div>
                </SortableContext>
              </DroppableContainer>
            </div>
          ))}
        </div>
      </DndContext>

      {(showEditModal || showCreateModal) && (
        <EditGoalModal
          goal={selectedGoal}
          onClose={handleModalClose}
          onSave={handleModalClose}
        />
      )}
    </div>
  );
};

export default GoalList;
