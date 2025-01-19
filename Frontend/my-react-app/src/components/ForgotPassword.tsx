import React, { useState } from "react";
import { Link } from "react-router-dom";

const ForgotPassword = () => {
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [error] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // This is just a placeholder - you'll need to implement the actual password reset functionality
    setMessage(
      "If an account exists with this email, you will receive password reset instructions."
    );
  };

  return (
    <div
      className="container d-flex align-items-center justify-content-center"
      style={{ minHeight: "90vh", background: "#1a1b2e" }}
    >
      <div className="w-100" style={{ maxWidth: "400px" }}>
        <form
          onSubmit={handleSubmit}
          className="p-4 rounded-lg"
          style={{ background: "#282a3e" }}
        >
          <h2 className="text-center mb-4 text-white">Forgot Password</h2>
          <p className="text-center mb-4" style={{ color: "#8b8c98" }}>
            Enter your email address and we'll send you instructions to reset
            your password.
          </p>

          {error && (
            <div className="alert alert-danger" role="alert">
              {error}
            </div>
          )}

          {message && (
            <div className="alert alert-success" role="alert">
              {message}
            </div>
          )}

          <div className="mb-4">
            <label htmlFor="email" className="form-label text-white">
              Email
            </label>
            <input
              type="email"
              id="email"
              className="form-control bg-transparent text-white border border-gray-600"
              style={{ background: "#1e1f2f" }}
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <button
            type="submit"
            className="btn w-100 mb-3"
            style={{ background: "#7c5cfc", color: "white" }}
          >
            Send Reset Instructions
          </button>

          <div className="text-center" style={{ color: "#8b8c98" }}>
            Remember your password?{" "}
            <Link
              to="/login"
              className="text-decoration-none"
              style={{ color: "#7c5cfc" }}
            >
              Back to Login
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ForgotPassword;
