/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Components/**/*.{razor,html,cshtml}",
        "./Pages/**/*.{razor,html,cshtml}",
        "./wwwroot/**/*.{html,js}"
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}

