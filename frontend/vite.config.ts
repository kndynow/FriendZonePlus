import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],

  // This is to silence Sass deprecation warnings from bootstrap. No long-term fix is in place yet: https://getbootstrap.com/docs/5.3/customize/sass/
  css: {
    preprocessorOptions: {
      scss: {
        silenceDeprecations: ["color-functions", "global-builtin", "import"],
      },
    },
  },
});
