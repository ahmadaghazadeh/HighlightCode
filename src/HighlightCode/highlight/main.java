SharedPreferences sharedPreferences = getSharedPreferences("Date", MODE_PRIVATE);
Map<String, ?> allEntries = sharedPreferences .getAll();
for (Map.Entry<String, ?> entry : allEntries.entrySet()) {
    Log.d("map values", entry.getKey() + ": " + entry.getValue().toString());
}  
