import React, { useEffect, useState } from "react";

// Define the User type
interface User {
  username: string;
  email: string;
}

const ProfilePage = () => {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    const fetchUserProfile = async () => {
      const token = localStorage.getItem("token");
      const response = await fetch("http://localhost:5129/api/user/profile", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      const data = await response.json();
      setUser(data);
    };

    fetchUserProfile();
  }, []);

  if (!user) return <div>Loading...</div>;

  return (
    <div>
      <h1>Profile Page</h1>
      <p>Username: {user.username}</p>
      <p>Email: {user.email}</p>
    </div>
  );
};

export default ProfilePage;
