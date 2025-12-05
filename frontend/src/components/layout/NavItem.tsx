import { NavLink } from "react-router-dom";

type NavItemProps = {
  to: string;
  icon: string;
};

export function NavItem({ to, icon }: NavItemProps) {
  function getNavItemClass(isActive: boolean): string {
    return isActive ? "nav-item active" : "nav-item";
  }

  function getIconClass(isActive: boolean): string {
    return isActive ? `bi bi-${icon}-fill` : `bi bi-${icon}`;
  }

  return (
    <NavLink to={to} className={({ isActive }) => getNavItemClass(isActive)}>
      {({ isActive }) => <i className={getIconClass(isActive)} />}
    </NavLink>
  );
}
