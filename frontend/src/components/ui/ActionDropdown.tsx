import { Dropdown } from "react-bootstrap";

export type ActionButton = {
  icon: string;
  onClick: () => void;
  label: string;
};

type ActionDropdownProps = {
  actions: ActionButton[];
};

export default function ActionDropdown({ actions }: ActionDropdownProps) {
  if (actions.length === 0) return null;

  return (
    <Dropdown>
      <Dropdown.Toggle
        variant="link"
        className="f-button fs-4 text-decoration-none p-0 border-0"
        style={{ boxShadow: "none" }}
      ></Dropdown.Toggle>
      <Dropdown.Menu align="end">
        {actions.map((action, index) => (
          <Dropdown.Item
            key={index}
            onClick={action.onClick}
            className="d-flex align-items-center gap-2"
          >
            <i className={action.icon}></i>
            <span>{action.label}</span>
          </Dropdown.Item>
        ))}
      </Dropdown.Menu>
    </Dropdown>
  );
}
