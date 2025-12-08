interface RegisterFormProps {
  onSwitchToLogin: () => void;
}

export default function RegisterForm({ onSwitchToLogin }: RegisterFormProps) {
  return (
    <>
      <h2>Registrera form ska vara här</h2>
      <p className="text-center mt-3">
        Har du redan ett konto?
        <button
          className="text-primary"
          style={{ cursor: "pointer" }}
          onClick={onSwitchToLogin}
        >
          Logga in här
        </button>
      </p>
    </>
  );
}
