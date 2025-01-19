import React, { useState } from "react";
import GoalForm from "../components/GoalForm";
import GoalList from "../components/GoalList";

const Goals: React.FC = () => {
  const [refreshKey, setRefreshKey] = useState(0);

  const handleGoalCreated = () => {
    setRefreshKey((prev) => prev + 1);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div className="md:order-2">
          <GoalForm onGoalCreated={handleGoalCreated} />
        </div>
        <div className="md:order-1">
          <GoalList key={refreshKey} />
        </div>
      </div>
    </div>
  );
};

export default Goals;
