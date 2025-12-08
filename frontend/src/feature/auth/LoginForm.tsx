interface LoginFormProps {
  onSwitchToRegister: () => void;
}

export default function LoginForm({ onSwitchToRegister }: LoginFormProps) {
  return (
    <>
      <h2>Logga in form ska vara här</h2>
      <p className="text-center mt-3">
        Inget konto?
        <button
          className="text-primary"
          style={{ cursor: "pointer" }}
          onClick={onSwitchToRegister}
        >
          Registrera här
        </button>
      </p>
    </>
  );
}
