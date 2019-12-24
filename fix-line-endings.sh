for file in $(find . -type f); do
   tr -d '\r' <$file >temp.$$ && mv temp.$$ $file
done
