import numpy as np
from sklearn.linear_model import LinearRegression
from sklearn.preprocessing import PolynomialFeatures
import matplotlib.pyplot as plt

# Data for 32:9 aspect ratio
attributes_32_9 = np.array([54.7682, 55.7536, 56.7626, 57.796, 58.8547, 59.9396, 61.0517, 62.1919, 63.3614, 64.5611,
                           65.7921, 67.0556, 68.3528, 69.685, 71.0533, 72.4591, 73.9037, 75.3885, 76.915, 78.4846,
                           80.0987, 81.7589, 83.4667])
fov_32_9 = np.array([123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141,
                     142, 143, 144, 145])

# Data for 16:9 aspect ratio
attributes_16_9 = np.array([54.5364, 55.3576, 56.1859, 57.0216, 57.8648, 58.7155, 59.574, 60.4404, 61.3148, 62.1974,
                           63.0883, 63.9877, 64.8956, 65.8124, 66.738, 67.6728, 68.6167, 69.57, 70.5328, 71.5053, 72.4876,
                           73.4799, 74.4824, 75.4951, 76.5183, 77.5521, 78.5967, 79.6522, 80.7188, 81.7965, 82.8857, 83.9863])
fov_16_9 = np.array([85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106,
                     107, 108, 109, 110, 111, 112, 113, 114, 115, 116])

# Data for 4:3 aspect ratio
attributes_4_3 = np.array([54.5387, 55.4129, 56.2909, 57.1725, 58.058, 58.9473, 59.8404, 60.7376, 61.6387, 62.5438,
                           63.4531, 64.3664, 65.284, 66.2059, 67.132, 68.0626, 68.9975, 69.9368, 70.8807, 71.8291,
                           72.7821, 73.7398, 74.7021, 75.6693, 76.6411, 77.6178, 78.5994, 79.5859, 80.5773, 81.5737,
                           82.5751, 83.5817])
fov_4_3 = np.array([69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92,
                   93, 94, 95, 96, 97, 98, 99, 100])

# Your fixed FOV value
fixed_fov = 114.0841655

# Outlier attributes.xml values
outlier_attributes = np.array([15, 120]) # Extremes that attributes.xml can read (fov unknown as they are locked to the known minimum/maximum)

# Reshape the data to meet scikit-learn requirements
attributes_32_9 = attributes_32_9.reshape(-1, 1)
attributes_16_9 = attributes_16_9.reshape(-1, 1)
attributes_4_3 = attributes_4_3.reshape(-1, 1)

# Create polynomial features
poly_degree = 2  # You can try different degrees
poly_features_32_9 = PolynomialFeatures(degree=poly_degree)
poly_attributes_32_9 = poly_features_32_9.fit_transform(attributes_32_9)

poly_features_16_9 = PolynomialFeatures(degree=poly_degree)
poly_attributes_16_9 = poly_features_16_9.fit_transform(attributes_16_9)

poly_features_4_3 = PolynomialFeatures(degree=poly_degree)
poly_attributes_4_3 = poly_features_4_3.fit_transform(attributes_4_3)

# Fit the model
model_32_9 = LinearRegression().fit(poly_attributes_32_9, fov_32_9)
model_16_9 = LinearRegression().fit(poly_attributes_16_9, fov_16_9)
model_4_3 = LinearRegression().fit(poly_attributes_4_3, fov_4_3)

# Predict using the model
predicted_fov_32_9 = model_32_9.predict(poly_attributes_32_9)
predicted_fov_16_9 = model_16_9.predict(poly_attributes_16_9)
predicted_fov_4_3 = model_4_3.predict(poly_attributes_4_3)

# Predict FOV for outlier attributes.xml values for 32:9 aspect ratio
outlier_attributes_32_9 = outlier_attributes.reshape(-1, 1)
poly_outlier_attributes_32_9 = poly_features_32_9.transform(outlier_attributes_32_9)
predicted_outlier_fov_32_9 = model_32_9.predict(poly_outlier_attributes_32_9)

# Predict FOV for outlier attributes.xml values for 16:9 aspect ratio
outlier_attributes_16_9 = outlier_attributes.reshape(-1, 1)
poly_outlier_attributes_16_9 = poly_features_16_9.transform(outlier_attributes_16_9)
predicted_outlier_fov_16_9 = model_16_9.predict(poly_outlier_attributes_16_9)

# Predict FOV for outlier attributes.xml values for 4:3 aspect ratio
outlier_attributes_4_3 = outlier_attributes.reshape(-1, 1)
poly_outlier_attributes_4_3 = poly_features_4_3.transform(outlier_attributes_4_3)
predicted_outlier_fov_4_3 = model_4_3.predict(poly_outlier_attributes_4_3)

# Prompt user to show outlier data or not
user_input = input("Do you want to show outlier data? (yes/no): ").lower()

# Update show_outliers based on user input
if user_input == 'yes':
    show_outliers = 'yes'
    extension_range = np.linspace(min(outlier_attributes), max(outlier_attributes), num=100).reshape(-1, 1)
    # Plot the outlier points for 32:9 aspect ratio
    plt.scatter(outlier_attributes, predicted_outlier_fov_32_9, label='Outlier Predicted FOV (32:9)', color='blue', marker='x')

    # Plot the outlier points for 16:9 aspect ratio
    plt.scatter(outlier_attributes, predicted_outlier_fov_16_9, label='Outlier Predicted FOV (16:9)', color='green', marker='x')

    #Plot the outlier points for 4:3 aspect ratio
    plt.scatter(outlier_attributes, predicted_outlier_fov_4_3, label='Outlier Predicted FOV (4:3)', color='red', marker='x')
else:
    extension_range = np.linspace(min(attributes_32_9), max(attributes_32_9), num=100).reshape(-1, 1)

# Generate predictions for the extension range for 32:9 aspect ratio
poly_extension_range_32_9 = poly_features_32_9.transform(extension_range)
predicted_extension_fov_32_9 = model_32_9.predict(poly_extension_range_32_9)

# Generate predictions for the extension range for 16:9 aspect ratio
poly_extension_range_16_9 = poly_features_16_9.transform(extension_range)
predicted_extension_fov_16_9 = model_16_9.predict(poly_extension_range_16_9)

# Generate predictions for the extension range for 4:3 aspect ratio
poly_extension_range_4_3 = poly_features_4_3.transform(extension_range)
predicted_extension_fov_4_3 = model_4_3.predict(poly_extension_range_4_3)

# Plot the results for 32:9 aspect ratio
plt.scatter(attributes_32_9, fov_32_9, label='32:9 Integer FOV degrees', color='blue')
plt.plot(attributes_32_9, predicted_fov_32_9, color='blue', linestyle='--', label='32:9 Predicted FOV')

# Plot the results for 16:9 aspect ratio
plt.scatter(attributes_16_9, fov_16_9, label='16:9 Integer FOV degrees', color='green')
plt.plot(attributes_16_9, predicted_fov_16_9, color='green', linestyle='--', label='16:9 Predicted FOV')

# Plot the results for 4:3 aspect ratioyes

plt.scatter(attributes_4_3, fov_4_3, label='4:3 Integer FOV degrees', color='red')
plt.plot(attributes_4_3, predicted_fov_4_3, color='red', linestyle='--', label='4:3 Predicted FOV')

# Draw the horizontal line for the fixed FOV
plt.axhline(y=fixed_fov, color='grey', linestyle='-', label=f'Valve Index FOV ({fixed_fov:.2f})')

# Plot the extension range for 32:9 aspect ratio
plt.plot(extension_range, predicted_extension_fov_32_9, color='blue', linestyle='--')

# Plot the extension range for 16:9 aspect ratio
plt.plot(extension_range, predicted_extension_fov_16_9, color='green', linestyle='--')

# Plot the extension range for 4:3 aspect ratio
plt.plot(extension_range, predicted_extension_fov_4_3, color='red', linestyle='--')

plt.xlabel('attributes.xml FOV value')
plt.ylabel('Game Actual FOV in Degrees')
plt.legend()
plt.show()