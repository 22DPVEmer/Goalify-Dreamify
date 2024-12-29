import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface User {
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
  isActive: boolean;
  isAdmin: boolean;
}

const Profile = () => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await fetch("http://localhost:5129/api/user/profile", {
          method: "GET",
          credentials: "include",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
          mode: "cors", // Add this
        });

        // For debugging
        const cookies = document.cookie;
        console.log("Cookies:", cookies);
        console.log("Response status:", response.status);
        console.log("Response headers:", response.headers);

        // Log the raw response
        const responseText = await response.text();
        console.log("Raw response:", responseText);

        if (!response.ok) {
          if (response.status === 401) {
            console.log("Unauthorized - redirecting to login");
            navigate("/login");
            return;
          }
          throw new Error(
            `HTTP error! status: ${response.status}, body: ${responseText}`
          );
        }

        // Parse the response text
        const data = JSON.parse(responseText);
        console.log("Profile data:", data);
        setUser(data);
      } catch (error) {
        console.error("Profile fetch error:", error);
        setError(
          error instanceof Error ? error.message : "Failed to load profile"
        );
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [navigate]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  if (!user) return <div>No user data found</div>;

  return (
    <div className="container mt-5">
      <div className="card">
        <div className="card-header">
          <h2>User Profile</h2>
        </div>
        <div className="card-body">
          <div className="row">
            <div className="col">
              <p>
                <strong>Username:</strong> {user.userName}
              </p>
              <p>
                <strong>Email:</strong> {user.email}
              </p>
              <p>
                <strong>First Name:</strong> {user.firstName}
              </p>
              <p>
                <strong>Last Name:</strong> {user.lastName}
              </p>
              <p>
                <strong>Account Status:</strong>{" "}
                {user.isActive ? "Active" : "Inactive"}
              </p>
              {user.isAdmin && (
                <p>
                  <strong>Role:</strong> Administrator
                </p>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
