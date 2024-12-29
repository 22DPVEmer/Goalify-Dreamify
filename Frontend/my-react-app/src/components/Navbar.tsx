import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const Navbar = () => {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    // Check authentication status
    checkAuth();
  }, []);

  const checkAuth = async () => {
    try {
      const response = await fetch("http://localhost:5129/api/auth/check", {
        credentials: "include",
      });
      setIsAuthenticated(response.ok);
    } catch (error) {
      setIsAuthenticated(false);
      console.error("Check auth error:", error);
    }
  };

  const handleLogout = async () => {
    try {
      await fetch("http://localhost:5129/api/auth/logout", {
        method: "POST",
        credentials: "include",
      });
      setIsAuthenticated(false);
      navigate("/login");
    } catch (error) {
      console.error("Logout error:", error);
    }
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light w-100">
      <div className="container-fluid">
        <a className="navbar-brand" href="/main">
          Goalify
        </a>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarContent"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarContent">
          <ul className="navbar-nav ms-auto">
            {isAuthenticated ? (
              <>
                <li className="nav-item">
                  <a className="nav-link" href="/profile">
                    Profile
                  </a>
                </li>
                <li className="nav-item">
                  <a className="nav-link" href="/journal">
                    Journal
                  </a>
                </li>
                <li className="nav-item">
                  <a className="nav-link" href="/dreamboard">
                    Dreamboard
                  </a>
                </li>
                <li className="nav-item">
                  <button
                    className="btn btn-outline-danger ms-2"
                    onClick={handleLogout}
                  >
                    Logout
                  </button>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <a className="nav-link" href="/login">
                    Login
                  </a>
                </li>
                <li className="nav-item">
                  <a className="nav-link" href="/register">
                    Register
                  </a>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
