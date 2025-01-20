# E-Commerce Order Management Frontend

This is the frontend application for the E-Commerce Order Management System. It's built with React, TypeScript, and modern web technologies.

## Features

- View and filter orders
- Create new orders with multiple items
- Cancel pending orders
- Real-time order status updates

## Tech Stack

- React 18 with TypeScript
- React Router for navigation
- React Query for data fetching
- React Hook Form for form handling
- TailwindCSS for styling
- Axios for API communication

## Getting Started

### Prerequisites

- Node.js 16 or later
- npm 7 or later

### Installation

1. Install dependencies:
```bash
npm install
```

2. Create a `.env` file in the root directory and add:
```bash
VITE_API_URL=http://localhost:5000
```

3. Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:5173`.

### Building for Production

To create a production build:

```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

## Development

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run lint` - Run ESLint
- `npm run preview` - Preview production build locally

### Project Structure

```
src/
  ├── api/          # API client and types
  ├── components/   # Reusable components
  ├── pages/        # Page components
  ├── App.tsx       # Main application component
  └── main.tsx      # Application entry point
```

## Contributing

1. Create a feature branch
2. Commit your changes
3. Push to the branch
4. Create a Pull Request

## License

This project is licensed under the MIT License.
