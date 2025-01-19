import React from "react";
import { useNavigate } from "react-router-dom";

const MainPage = () => {
  const navigate = useNavigate();

  const handleStartJournaling = () => {
    navigate("/register");
  };

  return (
    <div>
      <h1>Welcome to Goalify</h1>
      <p>
        Self-improvement is a journey that starts with setting goals and
        planning them out in a modern, user-friendly way. Start journaling today
        to track your progress and achieve your dreams.
      </p>
      <button onClick={handleStartJournaling} className="btn btn-primary">
        Start Journaling
      </button>
    </div>
  );
};

export default MainPage;
